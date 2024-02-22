using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;



namespace ClientPlugins.Client
{
    /// <summary>
    /// The ViewItemWpfUserControl is the WPF version of the ViewItemUserControl. It is instantiated for every position it is created on the current visible view. When a user select another View or ViewLayout, this class will be disposed.  No permanent settings can be saved in this class.
    /// The Init() method is called when the class is initiated and handle has been created for the UserControl. Please perform resource initialization in this method.
    /// <br>
    /// If Message communication is performed, register the MessageReceivers during the Init() method and UnRegister the receivers during the Close() method.
    /// <br>
    /// The Close() method can be used to Dispose resources in a controlled manor.
    /// <br>
    /// Mouse events not used by this control, should be passed on to the Smart Client by issuing the following methods:<br>
    /// FireClickEvent() for single click<br>
    ///	FireDoubleClickEvent() for double click<br>
    /// The single click will be interpreted by the Smart Client as a selection of the item, and the double click will be interpreted to expand the current viewitem to fill the entire View.
    /// </summary>
    public partial class ClientPluginsViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private ClientPluginsViewItemManager _viewItemManager;
        private object _themeChangedReceiver;

        // messaging
        private MessageCommunication _messageCommunication;
        private object _colorChange;



        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a ClientPluginsViewItemUserControl instance
        /// </summary>
        public ClientPluginsViewItemWpfUserControl(ClientPluginsViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

            SetHeaderColors();
        }

        private static Color GetWindowsMediaColor(System.Drawing.Color inColor)
        {
            return System.Windows.Media.Color.FromArgb(inColor.A, inColor.R, inColor.G, inColor.B);
        }

        private void SetHeaderColors()
        {
            _headerGrid.Background = new SolidColorBrush(GetWindowsMediaColor(ClientControl.Instance.Theme.BackgroundColor));
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _viewItemManager.PropertyChangedEvent += new EventHandler(ViewItemManagerPropertyChangedEvent);

            _themeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ThemeChangedIndicationHandler),
                                             new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));

            MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
            _messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);
            _colorChange = _messageCommunication.RegisterCommunicationFilter(ColorChangeHandler, new CommunicationIdFilter(ClientPluginsDefinition.ColorChange));
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            _viewItemManager.PropertyChangedEvent -= new EventHandler(ViewItemManagerPropertyChangedEvent);

            EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedReceiver);
            _themeChangedReceiver = null;

            EnvironmentManager.Instance.UnRegisterReceiver(_colorChange);
            _colorChange = null;
        }

        private object ColorChangeHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new MessageReceiver(ColorChangeHandler), message, dest, source);
            }
            else
            {
                try
                {
                    if (message.Data != null)
                    {
                        Color color = (Color)ColorConverter.ConvertFromString((string)message.Data);
                        SolidColorBrush brush = new SolidColorBrush(color);
                        panelMain.Background = brush;
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
            return null;

        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
            SetUpApplicationEventListeners();
            _nameTextBlock.Text = _viewItemManager.SomeName;
        }

        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            RemoveApplicationEventListeners();
        }

        #endregion

        #region Print method

        /// <summary>
        /// Method that is called when print is activated while the content holder is selected.
        /// </summary>
        public override void Print()
        {
            Print("Name of this item", "Some extra information");
        }

        #endregion

        #region Component events

        private void ViewItemWpfUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireClickEvent();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                FireRightClickEvent(e);
            }
        }

        private void ViewItemWpfUserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireDoubleClickEvent();
            }
        }

        /// <summary>
        /// Signals that the form is right clicked
        /// </summary>
        public event EventHandler RightClickEvent;

        /// <summary>
        /// Activates the RightClickEvent
        /// </summary>
        /// <param name="e">Event args</param>
        protected virtual void FireRightClickEvent(EventArgs e)
        {
            if (RightClickEvent != null)
            {
                RightClickEvent(this, e);
            }
        }

        void ViewItemManagerPropertyChangedEvent(object sender, EventArgs e)
        {
            _nameTextBlock.Text = _viewItemManager.SomeName;
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            SetHeaderColors();
            return null;
        }

        #endregion

        #region Component properties

        /// <summary>
        /// Gets boolean indicating whether the view item can be maximized or not. <br/>
        /// The content holder should implement the click and double click events even if it is not maximizable. 
        /// </summary>
        public override bool Maximizable
        {
            get { return true; }
        }

        /// <summary>
        /// Tell if ViewItem is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Make support for Theme colors to show if this ViewItem is selected or not.
        /// </summary>
        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                SetHeaderColors();
            }
        }

        #endregion

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            panelMain.Background = new SolidColorBrush(Colors.Red);
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            panelMain.Background = new SolidColorBrush(Colors.Blue);
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(
                    MessageId.SmartClient.ApplicationControlCommand
                    )
                { Data = ApplicationControlCommandData.Minimize });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            VideoOS.Platform.Messaging.Message colorChange = new VideoOS.Platform.Messaging.Message(ClientPluginsDefinition.ColorChange);
            //colorChange.Data = new SolidColorBrush(Colors.Red);
            /*
             * 위의 튜토리얼 코드로 실행하면  Message 'MESSAGESTRING' with prohibited data class type 'CLASS IDENTIFYING TEXT' discarded.오류남
            아래 아티클에서 상세 내용 확인 가능

            How to handle Event Server error 'CommunicationService.TransmitMessage' (troubleshooting)
            https://developer.milestonesys.com/s/article/how-to-handle-EVS-error-Message-x-with-prohibited-data-class-type
            */
            colorChange.Data = "RED";
            _messageCommunication.TransmitMessage(colorChange, null, null, null);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            VideoOS.Platform.Messaging.Message colorChange = new VideoOS.Platform.Messaging.Message(ClientPluginsDefinition.ColorChange);
            // colorChange.Data = (object) Colors.AliceBlue;
            /*
             * 위의 튜토리얼 코드로 실행하면  Message 'MESSAGESTRING' with prohibited data class type 'CLASS IDENTIFYING TEXT' discarded.오류남
            아래 아티클에서 상세 내용 확인 가능

            How to handle Event Server error 'CommunicationService.TransmitMessage' (troubleshooting)
            https://developer.milestonesys.com/s/article/how-to-handle-EVS-error-Message-x-with-prohibited-data-class-type
            */
            colorChange.Data = "BLUE";
            _messageCommunication.TransmitMessage(colorChange, null, null, null);
        }
    }
}
