‹

hC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.WinUI.Desktop\MainPage.xaml.cs
	namespace 	
Sandbox
 
. 
WinUI 
; 
public 
sealed 
partial 
class 
MainPage $
:% &
Page' +
{ 
public 

MainPage 
( 
) 
{		 
InitializeComponent

 
(

 
)

 
;

 
	WinUIPlot 
. 
	AppWindow 
= 
App !
.! "

MainWindow" ,
;, -
	WinUIPlot 
. 
Plot 
. 
Add 
. 
Signal !
(! "
Generate" *
.* +
Sin+ .
(. /
)/ 0
)0 1
;1 2
	WinUIPlot 
. 
Plot 
. 
Add 
. 
Signal !
(! "
Generate" *
.* +
Cos+ .
(. /
)/ 0
)0 1
;1 2
	WinUIPlot 
. 
Refresh 
( 
) 
; 
} 
} „"
cC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.WinUI.Desktop\App.xaml.cs
	namespace 	
Sandbox
 
. 
WinUI 
{		 
public 

partial 
class 
App 
: 
Application *
{ 
public 
App 
( 
) 
{ 	
this 
. 
InitializeComponent $
($ %
)% &
;& '
} 	
internal 
static 
Window 

MainWindow )
{   	
get!! 
{!! 
return!! 
_window!!  
!!!  !
;!!! "
}!!# $
private"" 
set"" 
{"" 
_window"" !
=""" #
value""$ )
;"") *
}""+ ,
}## 	
private$$ 
static$$ 
Window$$ 
?$$ 
_window$$ &
;$$& '
	protected++ 
override++ 
void++ 

OnLaunched++  *
(++* +
	Microsoft+++ 4
.++4 5
UI++5 7
.++7 8
Xaml++8 <
.++< =$
LaunchActivatedEventArgs++= U
args++V Z
)++Z [
{,, 	
if.. 
(.. 
System.. 
... 
Diagnostics.. "
..." #
Debugger..# +
...+ ,

IsAttached.., 6
)..6 7
{// 
}11 

MainWindow55 
=55 
new55 
Window55 #
(55# $
)55$ %
;55% &

MainWindow66 
.66 
Activate66 
(66  
)66  !
;66! "
if== 
(== 

MainWindow== 
.== 
Content== "
is==# %
not==& )
Frame==* /
	rootFrame==0 9
)==9 :
{>> 
	rootFrame@@ 
=@@ 
new@@ 
Frame@@  %
(@@% &
)@@& '
;@@' (
	rootFrameBB 
.BB 
NavigationFailedBB *
+=BB+ -
OnNavigationFailedBB. @
;BB@ A
ifDD 
(DD 
argsDD 
.DD '
UWPLaunchActivatedEventArgsDD 4
.DD4 5"
PreviousExecutionStateDD5 K
==DDL N%
ApplicationExecutionStateDDO h
.DDh i

TerminatedDDi s
)DDs t
{EE 
}GG 

MainWindowJJ 
.JJ 
ContentJJ "
=JJ# $
	rootFrameJJ% .
;JJ. /
}KK 
{PP 
ifQQ 
(QQ 
	rootFrameQQ 
.QQ 
ContentQQ %
==QQ& (
nullQQ) -
)QQ- .
{RR 
	rootFrameVV 
.VV 
NavigateVV &
(VV& '
typeofVV' -
(VV- .
MainPageVV. 6
)VV6 7
,VV7 8
argsVV9 =
.VV= >
	ArgumentsVV> G
)VVG H
;VVH I
}WW 

MainWindowYY 
.YY 
ActivateYY #
(YY# $
)YY$ %
;YY% &
}ZZ 
}[[ 	
voidbb 
OnNavigationFailedbb 
(bb  
objectbb  &
senderbb' -
,bb- .%
NavigationFailedEventArgsbb/ H
ebbI J
)bbJ K
{cc 	
throwdd 
newdd %
InvalidOperationExceptiondd /
(dd/ 0
$"dd0 2
$strdd2 A
{ddA B
eddB C
.ddC D
SourcePageTypeddD R
.ddR S
FullNameddS [
}dd[ \
$strdd\ ^
{dd^ _
edd_ `
.dd` a
	Exceptiondda j
}ddj k
"ddk l
)ddl m
;ddm n
}ee 	
privatenn 
voidnn 
OnSuspendingnn !
(nn! "
objectnn" (
sendernn) /
,nn/ 0
SuspendingEventArgsnn1 D
ennE F
)nnF G
{oo 	
varpp 
deferralpp 
=pp 
epp 
.pp 
SuspendingOperationpp 0
.pp0 1
GetDeferralpp1 <
(pp< =
)pp= >
;pp> ?
deferralrr 
.rr 
Completerr 
(rr 
)rr 
;rr  
}ss 	
}tt 
}uu 