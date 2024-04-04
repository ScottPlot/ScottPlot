ŠZ
cC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.WinUI\WinUIPlotMenu.cs
	namespace

 	
	ScottPlot


 
.

 
WinUI

 
;

 
public 
class 
WinUIPlotMenu 
: 
	IPlotMenu &
{ 
public 

string $
DefaultSaveImageFilename *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
=9 :
$str; E
;E F
public 

List 
< 
ContextMenuItem 
>  
ContextMenuItems! 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
=@ A
newB E
(E F
)F G
;G H
readonly 
	WinUIPlot 
ThisControl "
;" #
public 

WinUIPlotMenu 
( 
	WinUIPlot "
thisControl# .
). /
{ 
ThisControl 
= 
thisControl !
;! "
Reset 
( 
) 
; 
} 
public 

ContextMenuItem 
[ 
] &
GetDefaultContextMenuItems 7
(7 8
)8 9
{ 
ContextMenuItem 
	saveImage !
=" #
new$ '
(' (
)( )
{ 	
Label 
= 
$str  
,  !
OnInvoke 
= 
OpenSaveImageDialog *
,* +
} 	
;	 

ContextMenuItem   
	copyImage   !
=  " #
new  $ '
(  ' (
)  ( )
{!! 	
Label"" 
="" 
$str"" '
,""' (
OnInvoke## 
=##  
CopyImageToClipboard## +
,##+ ,
}$$ 	
;$$	 

return&& 
new&& 
ContextMenuItem&& "
[&&" #
]&&# $
{&&% &
	saveImage&&' 0
,&&0 1
	copyImage&&2 ;
}&&< =
;&&= >
}'' 
public)) 


MenuFlyout)) 
GetContextMenu)) $
())$ %
IPlotControl))% 1
plotControl))2 =
)))= >
{** 

MenuFlyout++ 
menu++ 
=++ 
new++ 
(++ 
)++ 
;++  
foreach-- 
(-- 
var-- 
curr-- 
in-- 
ContextMenuItems-- -
)--- .
{.. 	
if// 
(// 
curr// 
.// 
IsSeparator//  
)//  !
{00 
menu11 
.11 
Items11 
.11 
Add11 
(11 
new11 "
MenuFlyoutSeparator11# 6
(116 7
)117 8
)118 9
;119 :
}22 
else33 
{44 
var55 
menuItem55 
=55 
new55 "
MenuFlyoutItem55# 1
{552 3
Text554 8
=559 :
curr55; ?
.55? @
Label55@ E
}55F G
;55G H
menuItem66 
.66 
Click66 
+=66 !
(66" #
s66# $
,66$ %
e66& '
)66' (
=>66) +
curr66, 0
.660 1
OnInvoke661 9
(669 :
plotControl66: E
)66E F
;66F G
menu77 
.77 
Items77 
.77 
Add77 
(77 
menuItem77 '
)77' (
;77( )
}88 
}99 	
return;; 
menu;; 
;;; 
}<< 
public>> 

async>> 
void>> 
OpenSaveImageDialog>> )
(>>) *
IPlotControl>>* 6
plotControl>>7 B
)>>B C
{?? 
FileSavePicker@@ 
dialog@@ 
=@@ 
new@@  #
(@@# $
)@@$ %
{AA 	
SuggestedFileNameBB 
=BB $
DefaultSaveImageFilenameBB  8
}CC 	
;CC	 

dialogDD 
.DD 
FileTypeChoicesDD 
.DD 
AddDD "
(DD" #
$strDD# .
,DD. /
newDD0 3
ListDD4 8
<DD8 9
stringDD9 ?
>DD? @
(DD@ A
)DDA B
{DDC D
$strDDE K
}DDL M
)DDM N
;DDN O
dialogEE 
.EE 
FileTypeChoicesEE 
.EE 
AddEE "
(EE" #
$strEE# /
,EE/ 0
newEE1 4
ListEE5 9
<EE9 :
stringEE: @
>EE@ A
(EEA B
)EEB C
{EED E
$strEEF L
,EEL M
$strEEN U
}EEV W
)EEW X
;EEX Y
dialogFF 
.FF 
FileTypeChoicesFF 
.FF 
AddFF "
(FF" #
$strFF# .
,FF. /
newFF0 3
ListFF4 8
<FF8 9
stringFF9 ?
>FF? @
(FF@ A
)FFA B
{FFC D
$strFFE K
}FFL M
)FFM N
;FFN O
dialogGG 
.GG 
FileTypeChoicesGG 
.GG 
AddGG "
(GG" #
$strGG# /
,GG/ 0
newGG1 4
ListGG5 9
<GG9 :
stringGG: @
>GG@ A
(GGA B
)GGB C
{GGD E
$strGGF M
}GGN O
)GGO P
;GGP Q
dialogHH 
.HH 
FileTypeChoicesHH 
.HH 
AddHH "
(HH" #
$strHH# .
,HH. /
newHH0 3
ListHH4 8
<HH8 9
stringHH9 ?
>HH? @
(HH@ A
)HHA B
{HHC D
$strHHE K
}HHL M
)HHM N
;HHN O
varPP 
filePP 
=PP 
awaitPP 
dialogPP 
.PP  
PickSaveFileAsyncPP  1
(PP1 2
)PP2 3
;PP3 4
ifRR 

(RR 
fileRR 
!=RR 
nullRR 
)RR 
{SS 	
ImageFormatUU 
formatUU 
=UU  
ImageFormatLookupUU! 2
.UU2 3
FromFilePathUU3 ?
(UU? @
fileUU@ D
.UUD E
NameUUE I
)UUI J
;UUJ K
	PixelSizeVV 
lastRenderSizeVV $
=VV% &
plotControlVV' 2
.VV2 3
PlotVV3 7
.VV7 8
RenderManagerVV8 E
.VVE F

LastRenderVVF P
.VVP Q

FigureRectVVQ [
.VV[ \
SizeVV\ `
;VV` a
plotControlWW 
.WW 
PlotWW 
.WW 
SaveWW !
(WW! "
fileWW" &
.WW& '
PathWW' +
,WW+ ,
(WW- .
intWW. 1
)WW1 2
lastRenderSizeWW2 @
.WW@ A
WidthWWA F
,WWF G
(WWH I
intWWI L
)WWL M
lastRenderSizeWWM [
.WW[ \
HeightWW\ b
,WWb c
formatWWd j
)WWj k
;WWk l
}XX 	
}YY 
public[[ 

void[[  
CopyImageToClipboard[[ $
([[$ %
IPlotControl[[% 1
plotControl[[2 =
)[[= >
{\\ 
	PixelSize]] 
lastRenderSize]]  
=]]! "
plotControl]]# .
.]]. /
Plot]]/ 3
.]]3 4
RenderManager]]4 A
.]]A B

LastRender]]B L
.]]L M

FigureRect]]M W
.]]W X
Size]]X \
;]]\ ]
byte^^ 
[^^ 
]^^ 
bytes^^ 
=^^ 
plotControl^^ "
.^^" #
Plot^^# '
.^^' (
GetImage^^( 0
(^^0 1
(^^1 2
int^^2 5
)^^5 6
lastRenderSize^^6 D
.^^D E
Width^^E J
,^^J K
(^^L M
int^^M P
)^^P Q
lastRenderSize^^Q _
.^^_ `
Height^^` f
)^^f g
.^^g h
GetImageBytes^^h u
(^^u v
)^^v w
;^^w x
var`` 
stream`` 
=`` 
new`` &
InMemoryRandomAccessStream`` 3
(``3 4
)``4 5
;``5 6
streamaa 
.aa 
AsStreamForWriteaa 
(aa  
)aa  !
.aa! "
Writeaa" '
(aa' (
bytesaa( -
)aa- .
;aa. /
varcc 
contentcc 
=cc 
newcc 
DataPackagecc %
(cc% &
)cc& '
;cc' (
contentdd 
.dd 
	SetBitmapdd 
(dd '
RandomAccessStreamReferencedd 5
.dd5 6
CreateFromStreamdd6 F
(ddF G
streamddG M
)ddM N
)ddN O
;ddO P
	Clipboardff 
.ff 

SetContentff 
(ff 
contentff $
)ff$ %
;ff% &
}gg 
publicii 

voidii 
ShowContextMenuii 
(ii  
Pixelii  %
pixelii& +
)ii+ ,
{jj 

MenuFlyoutkk 
flyoutkk 
=kk 
GetContextMenukk *
(kk* +
ThisControlkk+ 6
)kk6 7
;kk7 8
Windowsll 
.ll 

Foundationll 
.ll 
Pointll  
ptll! #
=ll$ %
newll& )
(ll) *
pixelll* /
.ll/ 0
Xll0 1
,ll1 2
pixelll3 8
.ll8 9
Yll9 :
)ll: ;
;ll; <
flyoutmm 
.mm 
ShowAtmm 
(mm 
ThisControlmm !
,mm! "
ptmm# %
)mm% &
;mm& '
}nn 
publicpp 

voidpp 
Resetpp 
(pp 
)pp 
{qq 
Clearrr 
(rr 
)rr 
;rr 
ContextMenuItemsss 
.ss 
AddRangess !
(ss! "&
GetDefaultContextMenuItemsss" <
(ss< =
)ss= >
)ss> ?
;ss? @
}tt 
publicvv 

voidvv 
Clearvv 
(vv 
)vv 
{ww 
ContextMenuItemsxx 
.xx 
Clearxx 
(xx 
)xx  
;xx  !
}yy 
public{{ 

void{{ 
Add{{ 
({{ 
string{{ 
Label{{  
,{{  !
Action{{" (
<{{( )
IPlotControl{{) 5
>{{5 6
action{{7 =
){{= >
{|| 
ContextMenuItems}} 
.}} 
Add}} 
(}} 
new}}  
ContextMenuItem}}! 0
(}}0 1
)}}1 2
{}}3 4
Label}}5 :
=}}; <
Label}}= B
,}}B C
OnInvoke}}D L
=}}M N
action}}O U
}}}V W
)}}W X
;}}X Y
}~~ 
public
€€ 

void
€€ 
AddSeparator
€€ 
(
€€ 
)
€€ 
{
 
ContextMenuItems
‚‚ 
.
‚‚ 
Add
‚‚ 
(
‚‚ 
new
‚‚  
ContextMenuItem
‚‚! 0
(
‚‚0 1
)
‚‚1 2
{
‚‚3 4
IsSeparator
‚‚5 @
=
‚‚A B
true
‚‚C G
}
‚‚H I
)
‚‚I J
;
‚‚J K
}
ƒƒ 
}„„ á,
iC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.WinUI\WinUIPlotExtensions.cs
	namespace 	
	ScottPlot
 
. 
WinUI 
; 
internal 
static	 
class 
WinUIPlotExtensions )
{		 
internal

 
static

 
Pixel

 
Pixel

 
(

  
this

  $"
PointerRoutedEventArgs

% ;
e

< =
,

= >
	WinUIPlot

? H
plot

I M
)

M N
{ 
return 
e 
. 
GetCurrentPoint  
(  !
plot! %
)% &
.& '
Position' /
./ 0
ToPixel0 7
(7 8
)8 9
;9 :
} 
internal 
static 
Pixel 
ToPixel !
(! "
this" &
Point' ,
p- .
). /
{ 
return 
new 
Pixel 
( 
( 
float 
)  
p  !
.! "
X" #
,# $
(% &
float& +
)+ ,
p, -
.- .
Y. /
)/ 0
;0 1
} 
internal 
static 
Point 
ToPoint !
(! "
this" &
Pixel' ,
p- .
). /
{ 
return 
new 
Point 
( 
p 
. 
X 
, 
p 
.  
Y  !
)! "
;" #
} 
internal 
static 
Control 
. 
MouseButton '
ToButton( 0
(0 1
this1 5"
PointerRoutedEventArgs6 L
eM N
,N O
	WinUIPlotP Y
plotZ ^
)^ _
{ 
var 
props 
= 
e 
. 
GetCurrentPoint %
(% &
plot& *
)* +
.+ ,

Properties, 6
;6 7
switch 
( 
props 
. 
PointerUpdateKind '
)' (
{ 	
case 
PointerUpdateKind "
." #
MiddleButtonPressed# 6
:6 7
case 
PointerUpdateKind "
." # 
MiddleButtonReleased# 7
:7 8
return   
Control   
.   
MouseButton   *
.  * +
Middle  + 1
;  1 2
case!! 
PointerUpdateKind!! "
.!!" #
LeftButtonPressed!!# 4
:!!4 5
case"" 
PointerUpdateKind"" "
.""" #
LeftButtonReleased""# 5
:""5 6
return## 
Control## 
.## 
MouseButton## *
.##* +
Left##+ /
;##/ 0
case$$ 
PointerUpdateKind$$ "
.$$" #
RightButtonPressed$$# 5
:$$5 6
case%% 
PointerUpdateKind%% "
.%%" #
RightButtonReleased%%# 6
:%%6 7
return&& 
Control&& 
.&& 
MouseButton&& *
.&&* +
Right&&+ 0
;&&0 1
default'' 
:'' 
return(( 
Control(( 
.(( 
MouseButton(( *
.((* +
Unknown((+ 2
;((2 3
})) 	
}** 
internal,, 
static,, 
Control,, 
.,, 
Key,, 
Key,,  #
(,,# $
this,,$ (
KeyRoutedEventArgs,,) ;
e,,< =
),,= >
{-- 
return.. 
e.. 
... 
Key.. 
switch.. 
{// 	

VirtualKey00 
.00 
Control00 
=>00 !
Control00" )
.00) *
Key00* -
.00- .
Ctrl00. 2
,002 3

VirtualKey11 
.11 
LeftControl11 "
=>11# %
Control11& -
.11- .
Key11. 1
.111 2
Ctrl112 6
,116 7

VirtualKey22 
.22 
RightControl22 #
=>22$ &
Control22' .
.22. /
Key22/ 2
.222 3
Ctrl223 7
,227 8

VirtualKey44 
.44 
Menu44 
=>44 
Control44 &
.44& '
Key44' *
.44* +
Alt44+ .
,44. /

VirtualKey55 
.55 
LeftMenu55 
=>55  "
Control55# *
.55* +
Key55+ .
.55. /
Alt55/ 2
,552 3

VirtualKey66 
.66 
	RightMenu66  
=>66! #
Control66$ +
.66+ ,
Key66, /
.66/ 0
Alt660 3
,663 4

VirtualKey88 
.88 
Shift88 
=>88 
Control88  '
.88' (
Key88( +
.88+ ,
Shift88, 1
,881 2

VirtualKey99 
.99 
	LeftShift99  
=>99! #
Control99$ +
.99+ ,
Key99, /
.99/ 0
Shift990 5
,995 6

VirtualKey:: 
.:: 

RightShift:: !
=>::" $
Control::% ,
.::, -
Key::- 0
.::0 1
Shift::1 6
,::6 7
_<< 
=><< 
Control<< 
.<< 
Key<< 
.<< 
Unknown<< $
,<<$ %
}== 	
;==	 

}>> 
}?? íR
_C:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.WinUI\WinUIPlot.cs
	namespace 	
	ScottPlot
 
. 
WinUI 
; 
public

 
partial

 
class

 
	WinUIPlot

 
:

  
UserControl

! ,
,

, -
IPlotControl

. :
{ 
private 
readonly 
SKXamlCanvas !
_canvas" )
=* +
CreateRenderTarget, >
(> ?
)? @
;@ A
public 

Plot 
Plot 
{ 
get 
; 
} 
= 
new  #
(# $
)$ %
;% &
public 

	SkiaSharp 
. 
	GRContext 
? 
	GRContext  )
=>* ,
null- 1
;1 2
public 

IPlotInteraction 
Interaction '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 

	IPlotMenu 
Menu 
{ 
get 
;  
set! $
;$ %
}& '
public 

Window 
? 
	AppWindow 
{ 
get "
;" #
set$ '
;' (
}) *
public 

float 
DisplayScale 
{ 
get  #
;# $
set% (
;( )
}* +
public 

	WinUIPlot 
( 
) 
{ 
DisplayScale 
= 
DetectDisplayScale )
() *
)* +
;+ ,
Interaction 
= 
new 
Interaction %
(% &
this& *
)* +
;+ ,
Menu 
= 
new 
WinUIPlotMenu  
(  !
this! %
)% &
;& '

Background 
= 
new 
SolidColorBrush (
(( )
	Microsoft) 2
.2 3
UI3 5
.5 6
Colors6 <
.< =
White= B
)B C
;C D
_canvas!! 
.!! 
PaintSurface!! 
+=!! 
OnPaintSurface!!  .
;!!. /
_canvas## 
.## 
PointerWheelChanged## #
+=##$ &!
OnPointerWheelChanged##' <
;##< =
_canvas$$ 
.$$ 
PointerReleased$$ 
+=$$  "
OnPointerReleased$$# 4
;$$4 5
_canvas%% 
.%% 
PointerPressed%% 
+=%% !
OnPointerPressed%%" 2
;%%2 3
_canvas&& 
.&& 
PointerMoved&& 
+=&& 
OnPointerMoved&&  .
;&&. /
_canvas'' 
.'' 
DoubleTapped'' 
+='' 
OnDoubleTapped''  .
;''. /
_canvas(( 
.(( 
KeyDown(( 
+=(( 
	OnKeyDown(( $
;(($ %
_canvas)) 
.)) 
KeyUp)) 
+=)) 
OnKeyUp))  
;))  !
this++ 
.++ 
Content++ 
=++ 
_canvas++ 
;++ 
},, 
private.. 
static.. 
SKXamlCanvas.. 
CreateRenderTarget..  2
(..2 3
)..3 4
{// 
return00 
new00 
SKXamlCanvas00 
{11 	
VerticalAlignment22 
=22 
VerticalAlignment22  1
.221 2
Stretch222 9
,229 :
HorizontalAlignment33 
=33  !
HorizontalAlignment33" 5
.335 6
Stretch336 =
,33= >

Background44 
=44 
new44 
SolidColorBrush44 ,
(44, -
	Microsoft44- 6
.446 7
UI447 9
.449 :
Colors44: @
.44@ A
Transparent44A L
)44L M
}55 	
;55	 

}66 
public88 

void88 
Refresh88 
(88 
)88 
{99 
_canvas:: 
.:: 

Invalidate:: 
(:: 
):: 
;:: 
};; 
public== 

void== 
ShowContextMenu== 
(==  
Pixel==  %
position==& .
)==. /
{>> 
Menu?? 
.?? 
ShowContextMenu?? 
(?? 
position?? %
)??% &
;??& '
}@@ 
privateBB 
voidBB 
OnPaintSurfaceBB 
(BB  
objectBB  &
?BB& '
senderBB( .
,BB. /#
SKPaintSurfaceEventArgsBB0 G
eBBH I
)BBI J
{CC 
PlotDD 
.DD 
RenderDD 
(DD 
eDD 
.DD 
SurfaceDD 
.DD 
CanvasDD $
,DD$ %
(DD& '
intDD' *
)DD* +
eDD+ ,
.DD, -
SurfaceDD- 4
.DD4 5
CanvasDD5 ;
.DD; <
LocalClipBoundsDD< K
.DDK L
WidthDDL Q
,DDQ R
(DDS T
intDDT W
)DDW X
eDDX Y
.DDY Z
SurfaceDDZ a
.DDa b
CanvasDDb h
.DDh i
LocalClipBoundsDDi x
.DDx y
HeightDDy 
)	DD €
;
DD€ 
}EE 
privateGG 
voidGG 
OnPointerPressedGG !
(GG! "
objectGG" (
senderGG) /
,GG/ 0"
PointerRoutedEventArgsGG1 G
eGGH I
)GGI J
{HH 
FocusII 
(II 

FocusStateII 
.II 
PointerII  
)II  !
;II! "
InteractionKK 
.KK 
	MouseDownKK 
(KK 
eKK 
.KK  
PixelKK  %
(KK% &
thisKK& *
)KK* +
,KK+ ,
eKK- .
.KK. /
ToButtonKK/ 7
(KK7 8
thisKK8 <
)KK< =
)KK= >
;KK> ?
(MM 	
senderMM	 
asMM 
	UIElementMM 
)MM 
?MM 
.MM 
CapturePointerMM -
(MM- .
eMM. /
.MM/ 0
PointerMM0 7
)MM7 8
;MM8 9
baseOO 
.OO 
OnPointerPressedOO 
(OO 
eOO 
)OO  
;OO  !
}PP 
privateRR 
voidRR 
OnPointerReleasedRR "
(RR" #
objectRR# )
senderRR* 0
,RR0 1"
PointerRoutedEventArgsRR2 H
eRRI J
)RRJ K
{SS 
InteractionTT 
.TT 
MouseUpTT 
(TT 
eTT 
.TT 
PixelTT #
(TT# $
thisTT$ (
)TT( )
,TT) *
eTT+ ,
.TT, -
ToButtonTT- 5
(TT5 6
thisTT6 :
)TT: ;
)TT; <
;TT< =
(VV 	
senderVV	 
asVV 
	UIElementVV 
)VV 
?VV 
.VV !
ReleasePointerCaptureVV 4
(VV4 5
eVV5 6
.VV6 7
PointerVV7 >
)VV> ?
;VV? @
baseXX 
.XX 
OnPointerReleasedXX 
(XX 
eXX  
)XX  !
;XX! "
}YY 
private[[ 
void[[ 
OnPointerMoved[[ 
([[  
object[[  &
sender[[' -
,[[- ."
PointerRoutedEventArgs[[/ E
e[[F G
)[[G H
{\\ 
Interaction]] 
.]] 
OnMouseMove]] 
(]]  
e]]  !
.]]! "
Pixel]]" '
(]]' (
this]]( ,
)]], -
)]]- .
;]]. /
base^^ 
.^^ 
OnPointerMoved^^ 
(^^ 
e^^ 
)^^ 
;^^ 
}__ 
privateaa 
voidaa 
OnDoubleTappedaa 
(aa  
objectaa  &
senderaa' -
,aa- .'
DoubleTappedRoutedEventArgsaa/ J
eaaK L
)aaL M
{bb 
Interactioncc 
.cc 
DoubleClickcc 
(cc  
)cc  !
;cc! "
basedd 
.dd 
OnDoubleTappeddd 
(dd 
edd 
)dd 
;dd 
}ee 
privategg 
voidgg !
OnPointerWheelChangedgg &
(gg& '
objectgg' -
sendergg. 4
,gg4 5"
PointerRoutedEventArgsgg6 L
eggM N
)ggN O
{hh 
Interactionii 
.ii 
MouseWheelVerticalii &
(ii& '
eii' (
.ii( )
Pixelii) .
(ii. /
thisii/ 3
)ii3 4
,ii4 5
eii6 7
.ii7 8
GetCurrentPointii8 G
(iiG H
thisiiH L
)iiL M
.iiM N

PropertiesiiN X
.iiX Y
MouseWheelDeltaiiY h
)iih i
;iii j
basejj 
.jj !
OnPointerWheelChangedjj "
(jj" #
ejj# $
)jj$ %
;jj% &
}kk 
privatemm 
voidmm 
	OnKeyDownmm 
(mm 
objectmm !
sendermm" (
,mm( )
KeyRoutedEventArgsmm* <
emm= >
)mm> ?
{nn 
Interactionoo 
.oo 
KeyDownoo 
(oo 
eoo 
.oo 
Keyoo !
(oo! "
)oo" #
)oo# $
;oo$ %
basepp 
.pp 
	OnKeyDownpp 
(pp 
epp 
)pp 
;pp 
}qq 
privatess 
voidss 
OnKeyUpss 
(ss 
objectss 
senderss  &
,ss& '
KeyRoutedEventArgsss( :
ess; <
)ss< =
{tt 
Interactionuu 
.uu 
KeyUpuu 
(uu 
euu 
.uu 
Keyuu 
(uu  
)uu  !
)uu! "
;uu" #
basevv 
.vv 
OnKeyUpvv 
(vv 
evv 
)vv 
;vv 
}ww 
publicyy 

floatyy 
DetectDisplayScaleyy #
(yy# $
)yy$ %
{zz 
return}} 
$num}} 
;}} 
}~~ 
} 