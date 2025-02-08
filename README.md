上位机开发常用功能封装库，无任何第三方依赖，简单易用的Windows平台C#上位机及机器视觉开发常用功能封装库，并做到跨平台兼容。未列出所有功能，请自行体验。有问题请提issue。
- 该库为个人私用库，部分代码为初学C#早期时封装自网络，现绝大多数已经弃用，将在下个版本标记Obsolete，并尽快删除，如果侵权，请提issue，将即刻删除。

# Pcy.Common
通用库，使用netstandard2.0开发，并提供net8-windows版本，可跨平台使用，兼容.NET8及更新版本的AOT（XML自动配置不支持AOT）。

## 日志管理
Log类提供了日志记录和管理功能，支持不同级别的日志（如信息、警告、错误等）。
- 支持分级日志info,warnning,error,fatal
- 内部使用线程安全队列对日志进行排队，添加日志过程中无锁，几乎不损耗主程序运行时间
- Logger 日志管理类，提供日志功能
- LogBase 日志呈现基类，Logger的UI呈现基类，支持MVVM模式，兼容WPF和Avalonia

## 多线程
- ThreadPoolEx 系统ThreadPool的自我实现，内部使用自动维护动态扩容的线程池进行实现，可实现ThreadPool的所有功能。该库实现的目标是以尽可能快的速度执行并行任务，任务启动时间可控制在毫秒级。
  -（维护的线程池消耗完毕时，任务的启动时间略大于Thread.Start）
- TaskEx 系统Task的自我实现，基于ThreadPoolEx，可实现Task的大多数功能，支持await以及ConfigureAwait。该库实现的目标是以尽可能快的速度执行并行任务，经测试，从任务从启动到开始执行的时间可稳定在10ms内。

## 队列
Queue的简单抽象封装，推荐使用线程安全队列。
- ConFifoQueue单线程的线程安全队列抽象类
- MTFifoQueue多线程的线程安全队列抽象类
- FifoResultQueueBase基于ticket的队列，可在需要时使用添加任务时返回的ticket取对应的执行结果。

## 配置管理
使用XML实现自动生成和保存配置，支持常用类型的保存，支持List数据保存。**（基于反射，不支持AOT）**

## TCP通信
TCPClient和TCPServer
- 提供基于订阅机制的Socket TPC通信实现，内部使用Async实现
- 支持自动重连
- 使用事件处理数据：连接状态改变事件、收到数据事件
- Server支持多客户端，事件数据中提供客户端信息
- Client支持自动分包 **（请谨慎用于生成过程，未在生成过程中大批量测试）**

# Pcy.Windows
封装了Windows平台相关功能，所有功能均需要Windows平台支持，仅能在Windows平台下使用。

## 主要功能

### 1. Windows常用API封装

**ShellFileOperation** 类封装了 Windows 的文件操作 API（如复制、移动、删除文件等），并提供了简化后的接口来执行这些操作。

**ShellShortcut** 类提供了创建和管理桌面快捷方式的功能。

**TaskDialog** 类提供了创建复杂的任务对话框的功能，支持自定义按钮、单选按钮、超链接、进度条等。
- 提供了简易接口可快速实现类MessageBox形式的高级对话框弹出，net8及更新版本使用.NET API实现。
- 通过TaskProgressWattingDialog实现等待对话框，并可实时修改界面显示文本和进度条状态。

**MsgBox** 类提供了用于显示简单的模态消息框。

**Win32Api** 提供了常用的Windows API以及对应的参数封装，以及部分常用API的助手封装。

### 2. 配置文件管理

**Configuration** 提供了基于GetPrivateProfileString API的ini配置文件的简易读写，并提供了C#常用类型的快速封装。可通过隐藏类型IniConfigBase<T>自行拓展。
**AutoConfig** 提供基于繁盛的自动生成配置UI界面

### 3. 权限管理
**Authority** 提供了文件的权限管理系统，并提供了用户登录、用户管理等基础操作界面。支持Wpf的绑定。

### 4. 常用控件
提供了部分常用控件拓展
- AppClosing 类提供了在应用程序关闭时显示提示窗口的功能。
- ShowRenameDialog 快速输入单行文本
- WaitCursor 使用using自动控制鼠标变化

### 5. 日志管理

**日志管理** LogBase的WPF实现。
- 使用Wpf.LogUI类显示实时的日志UI
- 使用ElementHost .NET API可用于winform

## 6. WPF

**控件**
- 滚动文本MoveText
- 数字输入框NumberTextbox
- 基于canvas的无边界缩放控件ZoomViewer
- 日期时间选择器DataTimePicker（修改自：https://github.com/jiangyan219/DateTimePicker）
- 常用控件的主题
- 基于FlashWindowEx Win32 API的扩展方法
  
**转换器**
- 布尔转Visibility
- 枚举注释获取
- 反转布尔值
- 多绑定转Array数组
- 等等..
- 部分常用转换器在Converters类中提供静态实例

**Manager**
- 键盘焦点管理
- PasswordBox绑定
- PasswordBox水印
- TextBox水印

**MarkupExtensions**
- 枚举绑定获取（用于Commbox自动下拉）
- 图像灰度转换（Based on the version by Thomas LEBRUN (http://blogs.developpeur.org/tom)）
- SegoeMDL2图标拓展
- Shield按钮图标拓展

## 7.其它函数
- 开机自动启动
- 单例模式
- 管理员权限验证
- 文件字符合法性验证
- 窗口顶部弹出式通知
