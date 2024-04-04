Ò	
eC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Avalonia.Desktop\Program.cs
	namespace 	
Sandbox
 
. 
Avalonia 
. 
Desktop "
;" #
class 
Program 
{ 
[ 
	STAThread 
] 
public 

static 
void 
Main 
( 
string "
[" #
]# $
args% )
)) *
=>+ -
BuildAvaloniaApp. >
(> ?
)? @
. 	+
StartWithClassicDesktopLifetime	 (
(( )
args) -
)- .
;. /
public 

static 

AppBuilder 
BuildAvaloniaApp -
(- .
). /
=> 


AppBuilder 
. 
	Configure 
<  
App  #
># $
($ %
)% &
. 
UsePlatformDetect 
( 
)  
. 
WithInterFont 
( 
) 
. 

LogToTrace 
( 
) 
; 
} “
nC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Avalonia.Desktop\MainWindow.axaml.cs
	namespace 	
Sandbox
 
. 
Avalonia 
; 
public 
partial 
class 

MainWindow 
:  !
Window" (
{ 
public 


MainWindow 
( 
) 
{ 
InitializeComponent		 
(		 
)		 
;		 
}

 
} ì
lC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Avalonia.Desktop\MainView.axaml.cs
	namespace 	
Sandbox
 
. 
Avalonia 
; 
public 
partial 
class 
MainView 
: 
UserControl  +
{ 
public		 

MainView		 
(		 
)		 
{

 
InitializeComponent 
( 
) 
; 
AvaPlot 
. 
Plot 
. 
Add 
. 
Signal 
(  
Generate  (
.( )
Sin) ,
(, -
)- .
). /
;/ 0
AvaPlot 
. 
Plot 
. 
Add 
. 
Signal 
(  
Generate  (
.( )
Cos) ,
(, -
)- .
). /
;/ 0
AvaPlot 
. 
Refresh 
( 
) 
; 
} 
} †
gC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Avalonia.Desktop\App.axaml.cs
	namespace 	
Sandbox
 
. 
Avalonia 
{ 
public 

partial 
class 
App 
: 
Application *
{ 
public		 
override		 
void		 

Initialize		 '
(		' (
)		( )
{

 	
AvaloniaXamlLoader 
. 
Load #
(# $
this$ (
)( )
;) *
} 	
public 
override 
void .
"OnFrameworkInitializationCompleted ?
(? @
)@ A
{ 	
if 
( 
ApplicationLifetime #
is$ &3
'IClassicDesktopStyleApplicationLifetime' N
desktopO V
)V W
{ 
desktop 
. 

MainWindow "
=# $
new% (

MainWindow) 3
(3 4
)4 5
;5 6
} 
else 
if 
( 
ApplicationLifetime (
is) +*
ISingleViewApplicationLifetime, J
singleViewPlatformK ]
)] ^
{ 
singleViewPlatform "
." #
MainView# +
=, -
new. 1
MainView2 :
(: ;
); <
;< =
} 
base 
. .
"OnFrameworkInitializationCompleted 3
(3 4
)4 5
;5 6
} 	
} 
} 