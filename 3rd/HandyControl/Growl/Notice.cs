using System;
using System.Windows;
using System.Windows.Media;
using Pcy.Wpf.Growl;

namespace Pcy.Wpf
{
    public sealed class Notice : NotifyPropertyObject
    {
        public readonly static Geometry WarningGeometry = new PathGeometry(PathFigureCollection.Parse("M943.644188 827.215696l-351.176649-608.204749c-42.945473-74.36249-113.147387-74.36249-156.092861 0l-351.176649 608.204749c-42.946498 74.431167-7.811716 135.14955 78.012605 135.14955l702.420949 0C951.455904 962.36422 986.555836 901.645838 943.644188 827.215696zM466.187532 391.579035c12.621133-13.644108 28.66175-20.466675 48.233578-20.466675 19.580028 0 35.612444 6.75389 48.241778 20.194018 12.544256 13.473954 18.820484 30.325365 18.820484 50.587035 0 17.430551-26.19759 145.621205-34.929778 238.882082l-63.105666 0c-7.666162-93.259852-36.090106-221.450507-36.090106-238.882082C447.358847 421.938226 453.643275 405.155491 466.187532 391.579035zM561.76804 835.026386c-13.268949 12.928641-29.062535 19.375023-47.345906 19.375023-18.275171 0-34.076957-6.447407-47.346931-19.375023-13.235123-12.89379-19.818859-28.517221-19.818859-46.869269 0-18.249546 6.583736-34.043131 19.818859-47.278254 13.268949-13.235123 29.07176-19.852685 47.346931-19.852685 18.283371 0 34.076957 6.617562 47.345906 19.852685 13.235123 13.235123 19.827059 29.028709 19.827059 47.278254C581.595099 806.51019 575.003163 822.132597 561.76804 835.026386z"));
        public readonly static Geometry SuccessGeometry = new PathGeometry(PathFigureCollection.Parse("M512.66048 64.64c-247.424 0-448 200.57728-448 448s200.576 448 448 448 448-200.57728 448-448c0-247.424-200.57728-448-448-448z m250.71232 334.86336L480.98176 681.89312c-15.49568 15.49696-40.61952 15.49696-56.11648 0l-162.9184-162.9184c-15.49568-15.49568-15.49568-40.61824 0-56.1152s40.61952-15.49568 56.11648 0l134.85952 134.85952L707.25504 343.3856c15.49568-15.49568 40.61952-15.49568 56.11648 0s15.49696 40.6208 0.00128 56.11776z"));
        public readonly static Geometry InfoGeometry = new PathGeometry(PathFigureCollection.Parse("M505.6512 39.0144c-261.2224 3.4816-470.1184 218.112-466.6368 479.4368 3.4816 261.12 218.112 470.1184 479.3344 466.6368 261.2224-3.4816 470.1184-218.112 466.7392-479.3344C981.504 244.4288 766.8736 35.5328 505.6512 39.0144zM558.08 196.608c48.128 0 62.2592 27.9552 62.2592 59.8016 0 39.8336-31.9488 76.6976-86.3232 76.6976-45.568 0-67.1744-22.9376-65.9456-60.8256C468.0704 240.4352 494.7968 196.608 558.08 196.608zM434.7904 807.6288c-32.8704 0-56.9344-19.968-33.8944-107.6224l37.6832-155.5456c6.5536-24.8832 7.68-34.9184 0-34.9184-9.8304 0-52.5312 17.2032-77.7216 34.2016l-16.384-26.9312c79.9744-66.7648 171.8272-105.8816 211.2512-105.8816 32.8704 0 38.2976 38.912 21.9136 98.6112l-43.2128 163.5328c-7.68 28.8768-4.4032 38.912 3.2768 38.912 9.9328 0 42.1888-11.9808 73.9328-36.9664l18.6368 24.8832C552.5504 777.728 467.6608 807.6288 434.7904 807.6288z"));
        public readonly static Geometry ErrorGeometry = new PathGeometry(PathFigureCollection.Parse("M495.469714 0C224.621714 0 0 224.621714 0 495.469714c0 270.884571 224.621714 495.506286 495.469714 495.506286 270.884571 0 495.506286-224.621714 495.506286-495.506286C990.976 224.621714 766.354286 0 495.469714 0z m211.419429 634.221714c19.821714 19.821714 19.821714 46.226286 0 66.048s-46.226286 19.821714-66.048 0l-138.752-138.715428-145.334857 145.334857a51.858286 51.858286 0 0 1-72.667429 0 51.858286 51.858286 0 0 1 0-72.667429l145.334857-145.334857-138.752-138.752c-19.821714-19.821714-19.821714-46.226286 0-66.048s46.262857-19.821714 66.084572 0l138.715428 138.715429 145.371429-145.334857a51.858286 51.858286 0 0 1 72.667428 0 51.858286 51.858286 0 0 1 0 72.667428l-145.371428 145.334857 138.752 138.752z"));
        public readonly static Geometry AskGeometry = new PathGeometry(PathFigureCollection.Parse("M512 0 30.11843 240.941297l0 542.117406 481.88157 240.941297 481.88157-240.941297L993.88157 240.941297 512 0zM575.776472 768.799969 460.188012 768.799969 460.188012 656.222073l115.588459 0L575.776472 768.799969zM623.335603 509.329685c-52.375829 36.723353-59.600363 55.988096-59.600363 84.885211l0 19.866447L468.616977 614.081343l0-26.489278c0-45.754021 13.846342-80.67124 61.406497-116.791866 46.957428-36.723353 57.79423-62.0082 57.79423-84.282484 0-25.284848-21.67258-54.181962-55.386393-54.181962-42.743457 0-70.436142 26.489278-82.477374 85.486914l-105.956088-21.67258c24.683144-111.976192 82.477374-157.127486 205.289345-157.127486 98.12985 0 157.72919 63.212631 157.72919 131.842639C707.017407 423.240044 688.956071 461.76953 623.335603 509.329685z"));
        public readonly static Geometry FatalGeometry = new PathGeometry(PathFigureCollection.Parse("M716.8 375.466667l34.133333 34.133333c17.066667 17.066667 42.666667 17.066667 59.733334 0 17.066667-17.066667 17.066667-42.666667 0-59.733333l-34.133334-34.133334 34.133334-34.133333c17.066667-17.066667 17.066667-42.666667 0-59.733333-17.066667-17.066667-42.666667-17.066667-59.733334 0l-34.133333 34.133333-34.133333-34.133333c-17.066667-17.066667-42.666667-17.066667-59.733334 0-17.066667 17.066667-17.066667 42.666667 0 59.733333l34.133334 34.133333-34.133334 34.133334c-17.066667 17.066667-17.066667 42.666667 0 59.733333 17.066667 17.066667 42.666667 17.066667 59.733334 0l34.133333-34.133333z m-426.666667 0l34.133334 34.133333c17.066667 17.066667 42.666667 17.066667 59.733333 0 17.066667-17.066667 17.066667-42.666667 0-59.733333l-34.133333-34.133334 34.133333-34.133333c17.066667-17.066667 17.066667-42.666667 0-59.733333-17.066667-17.066667-42.666667-17.066667-59.733333 0l-34.133334 34.133333-34.133333-34.133333c-17.066667-17.066667-42.666667-17.066667-59.733333 0-17.066667 17.066667-17.066667 42.666667 0 59.733333l34.133333 34.133333-34.133333 34.133334c-17.066667 17.066667-17.066667 42.666667 0 59.733333 17.066667 17.066667 42.666667 17.066667 59.733333 0l34.133333-34.133333zM0 85.333333c0-51.2 42.666667-85.333333 85.333333-85.333333h853.333334c51.2 0 85.333333 42.666667 85.333333 85.333333v853.333334c0 51.2-42.666667 85.333333-85.333333 85.333333H85.333333c-51.2 0-85.333333-42.666667-85.333333-85.333333V85.333333z m512 469.333334c-136.533333 0-230.4 68.266667-290.133333 196.266666-8.533333 17.066667 0 42.666667 17.066666 59.733334s42.666667 0 59.733334-17.066667c51.2-93.866667 110.933333-145.066667 221.866666-145.066667 102.4 0 170.666667 51.2 221.866667 145.066667 8.533333 17.066667 34.133333 25.6 59.733333 17.066667 17.066667-8.533333 25.6-34.133333 17.066667-59.733334-76.8-128-170.666667-196.266667-307.2-196.266666z"));


        private readonly static SolidColorBrush WarnningBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e9af20"));
        private readonly static SolidColorBrush ErrorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#db3340"));
        private readonly static SolidColorBrush FatalBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f8491e"));

        static Notice()
        {
            WarningGeometry.Freeze();
            SuccessGeometry.Freeze();
            InfoGeometry.Freeze();
            ErrorGeometry.Freeze();
            AskGeometry.Freeze();
            FatalGeometry.Freeze();

            WarnningBrush.Freeze();
            ErrorBrush.Freeze();
            FatalBrush.Freeze();
        }

        /// <summary>
        /// 点击系统的关闭按钮，或是超时自动关闭时的按钮回调索引。
        /// </summary>
        public const int CLICK_SYS_CLOSE = int.MinValue + 1000;
        /// <summary>
        /// 参数错误时的索引。
        /// </summary>
        public const int CLICK_ERR = int.MinValue + 2000;

        /// <summary>
        /// 关闭前等待的最短时间。
        /// </summary>
        public const int MIN_WAIT_TIME = 1000;

        public Notice(string message)
        {
            Message = message;
        }

        /// <summary>
        /// 创建该信息的时间。
        /// </summary>
        public DateTime Time { get; } = DateTime.Now;
        /// <summary>
        /// 是否显示时间。
        /// </summary>
        public bool IsShowDateTime { get; set; } = true;



        private int _waitTimeMileseconds = 6000;

        /// <summary>
        /// 关闭通知框前的等待时间。
        /// 最小设定值为<see cref="MIN_WAIT_TIME"/>毫秒。
        /// </summary>
        public int WaitTimeMileseconds
        {
            get => _waitTimeMileseconds;
            set
            {
                _waitTimeMileseconds = Math.Max(value, MIN_WAIT_TIME);
            }
        }

        /// <summary>
        /// 显示的消息。
        /// </summary>
        public string Message { get; set; }


        private Geometry _icon;
        /// <summary>
        /// 提示图标。
        /// 如果图标为空，使用默认图标；否则使用自定义图标。
        /// </summary>
        public Geometry Icon
        {
            get
            {
                if (_icon != null)
                    return _icon;
                switch (InfoType)
                {
                    case InfoType.Success:
                        return SuccessGeometry;
                    case InfoType.Warning:
                        return WarningGeometry;
                    case InfoType.Error:
                        return ErrorGeometry;
                    case InfoType.Fatal:
                        return FatalGeometry;
                    case InfoType.Ask:
                        return AskGeometry;
                    case InfoType.Info:
                    default:
                        return InfoGeometry;
                }
            }
            set
            {
                _icon = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// 提示图标的背景色。
        /// </summary>
        public Brush IconBrush { get; set; }

        /// <summary>
        /// 点击按钮时的回调方法。
        /// 点击的按钮索引和按钮名称相关，如果按钮名称相同，则返回第一个同名按钮的索引值。
        /// </summary>
        public NoticeClickCallback ClickCallback { get; set; }

        /// <summary>
        /// 自定义按钮，如果未设置点击回调方法<see cref="ClickCallback"/>，将不会显示按钮。
        /// </summary>
        public string[] Buttons { get; set; }

        public string CloseStr { get; set; } = "关闭";

        public bool StaysOpen { get; set; }

        public bool IsCustom { get; set; }

        public InfoType InfoType { get; set; }

        public bool ShowCloseButton { get; set; } = true;

        public FlowDirection FlowDirection { get; set; }

        public void Show(bool global = false)
        {
            if (global)
            {
                NoticeCtrl.ShowGlobal(this);
            }
            else
            {
                NoticeCtrl.Show(this);
            }
        }


        #region 显示消息提示
        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="message"></param>
        public static void Success(string message, bool global = false) => Success(new Notice(message)
        {
        }, false);

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Success(Notice growlInfo, bool global = false)
        {
            growlInfo.InfoType = InfoType.Success;
            growlInfo.Show(global);
        }


        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message, bool global = false) => Info(new Notice(message)
        {
            IconBrush = Brushes.Green
        }, global);

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Info(Notice growlInfo, bool global = false)
        {
            growlInfo.InfoType = InfoType.Info;
            growlInfo.Show(global);
        }

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(string message, bool global) => Warning(new Notice(message)
        {
            IconBrush = WarnningBrush,
        }, global);

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Warning(Notice growlInfo, bool global = false)
        {
            growlInfo.InfoType = InfoType.Warning;
            growlInfo.Show(global);
        }


        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message, bool global) => Error(new Notice(message)
        {
            IconBrush = ErrorBrush,
        }, false);

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Error(Notice growlInfo, bool global = false)
        {
            growlInfo.InfoType = InfoType.Error;
            growlInfo.Show(global);
        }


        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        public static void Fatal(string message, bool global = false) => Fatal(new Notice(message)
        {
            IconBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#326cf3")),
        }, global);

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Fatal(Notice growlInfo, bool global = false)
        {
            growlInfo.InfoType = InfoType.Fatal;
            growlInfo.Show(global);
        }


        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clickCallback">点击回调函数，输入值表示点击按钮类型，返回值表示是否关闭窗口。</param>
        /// <param name="global"></param>
        public static void Ask(string message, NoticeClickCallback clickCallback, bool stayOpen = true, bool global = false)
        {
            Ask(new Notice(message)
            {
                Buttons = new string[] { "确定", "取消" },
                ClickCallback = clickCallback,
                IconBrush = FatalBrush,
                StaysOpen = stayOpen
            }, global);
        }

        public static void Ask(Notice growlInfo, bool global = false)
        {
            growlInfo.InfoType = InfoType.Ask;
            growlInfo.Show(global);
        }

        /// <summary>
        /// 推送一个自定义通知。
        /// </summary>
        /// <param name="message">通知内容</param>
        /// <param name="infoType">通知显示的图标</param>
        /// <param name="buttons">通知拥有的按钮</param>
        /// <param name="clickCallback">点击按钮时的回调函数</param>
        /// <param name="stayOpen">是否一直显示通知</param>
        /// <param name="global">是否全局显示通知</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Push(string message, InfoType infoType, string[] buttons, NoticeClickCallback clickCallback, bool stayOpen = true, bool global = false)
        {
            if (buttons.Length < 1)
            {
                throw new ArgumentException("通知至少需要传递一个按钮。");
            }
            if (clickCallback == null)
            {
                throw new ArgumentNullException(nameof(clickCallback), "按钮按下的回调函数不能为空。");
            }
            var info = new Notice(message)
            {
                Buttons = buttons,
                ClickCallback = clickCallback,
                StaysOpen = stayOpen,
                InfoType = infoType
            };
            info.Show(global);
        }
        #endregion
    }

    /// <summary>
    /// 按下通知回调方法。
    /// 输入值为按下的按钮索引，返回值为是否关闭弹出窗口，返回值不控制自动关闭。
    /// 如果需要阻止自动关闭，请设置<see cref="Notice.StaysOpen"/>。
    /// </summary>
    /// <param name="btnIndex">按下的按钮索引，系统关闭时，按钮索引为 <see cref="Notice.CLICK_SYS_CLOSE"/></param>
    /// <returns></returns>
    public delegate bool NoticeClickCallback(int btnIndex);


    public enum InfoType
    {
        Success = 0,
        Info,
        Warning,
        Error,
        Fatal,
        Ask
    }
}
