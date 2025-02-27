using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Pcy.Common;
using Pcy.Wpf.MarkupExtensions;

namespace Pcy.Wpf.Growl
{
    /// <summary>
    ///     消息提醒
    /// </summary>
    [TemplatePart(Name = ElementPanelMore, Type = typeof(Panel))]
    [TemplatePart(Name = ElementGridMain, Type = typeof(Grid))]
    [TemplatePart(Name = ElementButtonClose, Type = typeof(Button))]
    internal sealed class NoticeCtrl : Control
    {
        private const string ElementPanelMore = "PART_PanelMore";
        private const string ElementGridMain = "PART_GridMain";
        private const string ElementButtonClose = "PART_ButtonClose";

        /// <summary>
        /// 全局显示的静态窗口
        /// </summary>
        private static NoticeGWindow GrowlWindow;

        private readonly static Dictionary<string, Panel> PanelDic = new Dictionary<string, Panel>();

        private FrameworkElement _panelMore;
        private Grid _gridMain;
        private Button _buttonClose;


        public static RoutedCommand GenericCmd { get; } = new RoutedCommand(nameof(GenericCmd), typeof(NoticeCtrl));

        static NoticeCtrl()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NoticeCtrl), new FrameworkPropertyMetadata(typeof(NoticeCtrl)));
        }


        /// <summary>
        /// 通知信息
        /// </summary>
        private readonly Notice NoticeInfo;


        private NoticeCtrl(Notice growlInfo)
        {
            NoticeInfo = growlInfo ?? throw new ArgumentNullException(nameof(growlInfo));
            this.DataContext = growlInfo;

            CommandBindings.Add(new CommandBinding(GenericCmd, (ss, ee) =>
            {
                if (ee.Parameter is int intVal)
                {
                    Close(intVal);
                }
                else if (growlInfo.Buttons != null && ee.Parameter is string)
                {
                    Close(Array.IndexOf(growlInfo.Buttons, ee.Parameter));
                }
                else
                {
                    Close(Notice.CLICK_ERR);
                }
            }));
        }

        #region 重载方法
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (NoticeInfo.ShowCloseButton)
            {
                _buttonClose.Visibility = Visibility.Visible;
            }
            else
            {
                _buttonClose.Visibility = Visibility.Collapsed;
            }
            //鼠标进入时停止关闭倒计时
            _timerClose?.Stop();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _buttonClose.Visibility = Visibility.Collapsed;

            //鼠标离开时重新开始计时关闭操作
            _timerClose?.Start();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _panelMore = GetTemplateChild(ElementPanelMore) as FrameworkElement;
            _gridMain = GetTemplateChild(ElementGridMain) as Grid;
            _buttonClose = GetTemplateChild(ElementButtonClose) as Button;

            Update();
        }
        #endregion




        private static void InitGrowlPanel(Panel panel)
        {
            if (panel == null)
                return;

            var menuItem = new MenuItem()
            {
                Header = "清除所有通知",
                Icon = SegoeMDL2IconExtension.GetSegoeMDL2Icon("\uEA99", 18, Brushes.SteelBlue)
            };

            menuItem.Click += (s, e) =>
            {
                foreach (var item in panel.Children.OfType<NoticeCtrl>())
                {
                    item.Close(Notice.CLICK_SYS_CLOSE);
                }
            };
            panel.ContextMenu = new ContextMenu
            {
                Items =
            {
                menuItem
            }
            };
        }
        private static DoubleAnimation CreateAnimation(double toValue, double milliseconds = 200)
        {
            return new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        /// <summary>
        /// 是否显示更多按钮项目。
        /// </summary>
        private bool ShowMoreItem => NoticeInfo.ClickCallback != null && NoticeInfo.Buttons?.Length >= 1;

        /// <summary>
        /// 关闭前等待计时器
        /// </summary>
        private System.Timers.Timer _timerClose;

        private void Update()
        {
            if (_panelMore == null || _gridMain == null || _buttonClose == null)
                throw new Exception();

            if (ShowMoreItem)
            {
                _panelMore.IsEnabled = true;
                _panelMore.Visibility = Visibility.Visible;
            }

            var transform = new TranslateTransform
            {
                X = FlowDirection == FlowDirection.LeftToRight ? MaxWidth : -MaxWidth
            };
            _gridMain.RenderTransform = transform;
            transform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(0, 200));

            if (!NoticeInfo.StaysOpen)
            {
                _timerClose = new System.Timers.Timer(NoticeInfo.WaitTimeMileseconds);

                _timerClose.Elapsed += delegate
                {
                    this.Dispatcher.BeginInvoke(new Action(() => Close(Notice.CLICK_SYS_CLOSE)));
                };

                _timerClose.Start();
            }
        }

        private static void ShowInternal(Panel panel, NoticeCtrl growl)
        {
            if (panel is null)
            {
                return;
            }

            var items = panel.Children.OfType<NoticeCtrl>();

            if (items.Any())
            {
                var transform = new TranslateTransform();
                // 为现有子项应用 TranslateTransform
                foreach (NoticeCtrl child in items)
                {
                    child.RenderTransform = transform;
                }
                //判断是否需要显示更多按钮来控制移动高度：移动高度只和新加入的项目高度有关
                var height = growl.ShowMoreItem ? 82.48d : 60d;

                //添加动画
                var animal = CreateAnimation(height, 150);
                //创建本地函数
                void handler(object ss, EventArgs ee)
                {
                    //添加新项目
                    panel.Children.Insert(0, growl);

                    //将已有控件补偿位置还原
                    foreach (var item in items)
                    {
                        item.RenderTransform = null;
                    }
                    animal.Completed -= handler;
                }
                animal.Completed += handler;
                transform.BeginAnimation(TranslateTransform.YProperty, animal);
            }
            else
            {
                panel.Children.Insert(0, growl);
            }
        }

        public static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
                return default;
            if (d is T t)
                return t;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = GetChild<T>(child);
                if (result != null)
                    return result;
            }

            return default;
        }

        public static T GetParent<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
            {
                return default;
            }
            if (d is T)
            {
                return d as T;
            }
            if (d is Window)
            {
                return null;
            }
            return GetParent<T>(VisualTreeHelper.GetParent(d));
        }

        private static void RemoveDefaultPanel(Panel panel)
        {
            FrameworkElement element = Exts.GetActiveWindow();
            var decorator = GetChild<AdornerDecorator>(element);

            if (decorator != null)
            {
                var layer = decorator.AdornerLayer;
                var adorner = GetParent<AdornerContainer>(panel);

                if (adorner != null && adorner != null)
                {
                    layer?.Remove(adorner);
                }
            }
        }



        #region 消息发送
        internal static void ShowGlobal(Notice growlInfo)
        {
            Application.Current.Dispatcher?.BeginInvoke(() =>
            {
                if (GrowlWindow == null)
                {
                    GrowlWindow = new NoticeGWindow();
                    GrowlWindow.Show();
                    InitGrowlPanel(GrowlWindow.GrowlPanel);
                    GrowlWindow.Init();
                }

                GrowlWindow.Visibility = Visibility.Visible;

                var ctl = new NoticeCtrl(growlInfo)
                {
                    FlowDirection = growlInfo.FlowDirection
                };

                ShowInternal(GrowlWindow.GrowlPanel, ctl);
            });
        }

        /// <summary>
        ///     显示信息
        /// </summary>
        /// <param name="growlInfo"></param>
        internal static void Show(Notice growlInfo)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                var ctl = new NoticeCtrl(growlInfo);
                ShowInternal(GetOrCreateCurrentPannel(), ctl);
            });
        }
        private static Panel GetOrCreateCurrentPannel()
        {
            FrameworkElement element = Exts.GetActiveWindow();
            var decorator = GetChild<AdornerDecorator>(element);

            if (decorator != null)
            {
                var layer = decorator.AdornerLayer;

                if (layer != null)
                {
                    var adorners = layer.GetAdorners(layer);
                    if (adorners == null)
                    {
                        goto CREATE_PANNEL;
                    }
                    var already = adorners.OfType<AdornerContainer>().FirstOrDefault();
                    if (already == null)
                    {
                        goto CREATE_PANNEL;
                    }
                    if (already.Child is not ScrollViewer scroll)
                    {
                        goto CREATE_PANNEL;
                    }
                    if (scroll.Content is not Panel pannelAlready)
                    {
                        goto CREATE_PANNEL;
                    }
                    return pannelAlready;


                CREATE_PANNEL:
                    var panel = new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Top
                    };

                    InitGrowlPanel(panel);

                    var scrollViewer = new ScrollViewer
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                        //IsInertiaEnabled = true,
                        //IsPenetrating = true,
                        Content = panel
                    };

                    var container = new AdornerContainer(layer)
                    {
                        Child = scrollViewer
                    };

                    layer.Add(container);

                    return panel;
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 关闭
        /// </summary>
        private void Close(int invokeParam)
        {
            if (NoticeInfo.ClickCallback != null)
            {
                if (NoticeInfo.ClickCallback.Invoke(invokeParam) == false)
                {
                    return;
                }
            }

            if (_timerClose != null)
            {
                _timerClose.Stop();
                _timerClose.Dispose();
                _timerClose = null;
            }

            var transform = new TranslateTransform();
            _gridMain.RenderTransform = transform;

            var animation = new DoubleAnimation(FlowDirection == FlowDirection.LeftToRight ? ActualWidth : -ActualWidth, new Duration(TimeSpan.FromMilliseconds(200)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };

            animation.Completed += (s, e) =>
            {
                if (Parent is Panel panel)
                {
                    if (GetTopLevelWindow(this) is NoticeGWindow gWindow)
                    {
                        panel.Children.Remove(this);

                        if (GrowlWindow != null)
                        {
                            if (gWindow.GrowlPanel != null && gWindow.GrowlPanel.Children.Count == 0)
                            {
                                gWindow.Close();
                                if (gWindow == GrowlWindow)
                                {
                                    GrowlWindow = null;
                                }
                            }

                        }
                    }
                    else
                    {
                        panel.Children.Remove(this);

                        if (panel.Children.Count == 0)
                        {
                            RemoveDefaultPanel(panel);
                        }
                    }
                }
            };

            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        public static Window GetTopLevelWindow(UIElement element)
        {
            // 从控件开始，一直查找直到找到顶层窗口
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            while (parent != null)
            {
                if (parent is Window window)
                {
                    return window;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null; // 如果没有找到，返回 null
        }

        /// <summary>
        ///     清除
        /// </summary>
        public static void ClearGlobal()
        {
            if (GrowlWindow == null)
                return;

            GrowlWindow.GrowlPanel?.Children.Clear();

            GrowlWindow.Close();
            GrowlWindow = null;
        }
    }
}

