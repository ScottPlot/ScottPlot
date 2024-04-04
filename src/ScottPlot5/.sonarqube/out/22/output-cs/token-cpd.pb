€P
dC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Avalonia\AvaPlotMenu.cs
	namespace 	
	ScottPlot
 
. 
Avalonia 
; 
public		 
class		 
AvaPlotMenu		 
:		 
	IPlotMenu		 $
{

 
public 

string $
DefaultSaveImageFilename *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
=9 :
$str; E
;E F
public 

List 
< 
ContextMenuItem 
>  
ContextMenuItems! 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
=@ A
newB E
(E F
)F G
;G H
private 
readonly 
AvaPlot 
ThisControl (
;( )
public 

AvaPlotMenu 
( 
AvaPlot 
avaPlot &
)& '
{ 
ThisControl 
= 
avaPlot 
; 
Reset 
( 
) 
; 
} 
public 

ContextMenuItem 
[ 
] &
GetDefaultContextMenuItems 7
(7 8
)8 9
{ 
ContextMenuItem 
	saveImage !
=" #
new$ '
(' (
)( )
{ 	
Label 
= 
$str  
,  !
OnInvoke 
= 
OpenSaveImageDialog *
} 	
;	 

return   
new   
ContextMenuItem   "
[  " #
]  # $
{  % &
	saveImage  ' 0
}  1 2
;  2 3
}!! 
public## 

ContextMenu## 
GetContextMenu## %
(##% &
)##& '
{$$ 
List%% 
<%% 
MenuItem%% 
>%% 
items%% 
=%% 
new%% "
(%%" #
)%%# $
;%%$ %
foreach'' 
('' 
var'' 
curr'' 
in'' 
ContextMenuItems'' -
)''- .
{(( 	
if)) 
()) 
curr)) 
.)) 
IsSeparator))  
)))  !
{** 
items++ 
.++ 
Add++ 
(++ 
new++ 
MenuItem++ &
{++' (
Header++) /
=++0 1
$str++2 5
}++6 7
)++7 8
;++8 9
},, 
else-- 
{.. 
var// 
menuItem// 
=// 
new// "
MenuItem//# +
{//, -
Header//. 4
=//5 6
curr//7 ;
.//; <
Label//< A
}//B C
;//C D
menuItem00 
.00 
Click00 
+=00 !
(00" #
s00# $
,00$ %
e00& '
)00' (
=>00) +
curr00, 0
.000 1
OnInvoke001 9
(009 :
ThisControl00: E
)00E F
;00F G
items11 
.11 
Add11 
(11 
menuItem11 "
)11" #
;11# $
}22 
}33 	
return55 
new55 
(55 
)55 
{66 	
ItemsSource77 
=77 
items77 
}88 	
;88	 

}99 
public;; 

async;; 
void;; 
OpenSaveImageDialog;; )
(;;) *
IPlotControl;;* 6
plotControl;;7 B
);;B C
{<< 
var== 
topLevel== 
=== 
TopLevel== 
.==  
GetTopLevel==  +
(==+ ,
ThisControl==, 7
)==7 8
??==9 ;
throw==< A
new==B E"
NullReferenceException==F \
(==\ ]
$str==] y
)==y z
;==z {
var>> 
destinationFile>> 
=>> 
await>> #
topLevel>>$ ,
.>>, -
StorageProvider>>- <
.>>< =
SaveFilePickerAsync>>= P
(>>P Q
new>>Q T!
FilePickerSaveOptions>>U j
(>>j k
)>>k l
{?? 	
SuggestedFileName@@ 
=@@ $
DefaultSaveImageFilename@@  8
,@@8 9
FileTypeChoicesAA 
=AA 
filePickerFileTypesAA 1
}BB 	
)BB	 

;BB
 
stringDD 
?DD 
pathDD 
=DD 
destinationFileDD &
?DD& '
.DD' (
TryGetLocalPathDD( 7
(DD7 8
)DD8 9
;DD9 :
ifEE 

(EE 
pathEE 
isEE 
notEE 
nullEE 
&&EE 
!EE  !
stringEE! '
.EE' (
IsNullOrWhiteSpaceEE( :
(EE: ;
pathEE; ?
)EE? @
)EE@ A
{FF 	
	PixelSizeGG 
lastRenderSizeGG $
=GG% &
plotControlGG' 2
.GG2 3
PlotGG3 7
.GG7 8
RenderManagerGG8 E
.GGE F

LastRenderGGF P
.GGP Q

FigureRectGGQ [
.GG[ \
SizeGG\ `
;GG` a
plotControlHH 
.HH 
PlotHH 
.HH 
SaveHH !
(HH! "
pathHH" &
,HH& '
(HH( )
intHH) ,
)HH, -
lastRenderSizeHH- ;
.HH; <
WidthHH< A
,HHA B
(HHC D
intHHD G
)HHG H
lastRenderSizeHHH V
.HHV W
HeightHHW ]
,HH] ^
ImageFormatLookupHH_ p
.HHp q
FromFilePathHHq }
(HH} ~
path	HH~ ‚
)
HH‚ ƒ
)
HHƒ „
;
HH„ …
}II 	
}JJ 
publicLL 

readonlyLL 
ListLL 
<LL 
FilePickerFileTypeLL +
>LL+ ,
filePickerFileTypesLL- @
=LLA B
newLLC F
(LLF G
)LLG H
{MM 
newNN 
(NN 
$strNN 
)NN 
{NN 
PatternsNN #
=NN$ %
newNN& )
ListNN* .
<NN. /
stringNN/ 5
>NN5 6
{NN7 8
$strNN9 @
}NNA B
}NNC D
,NND E
newOO 
(OO 
$strOO 
)OO 
{OO 
PatternsOO $
=OO% &
newOO' *
ListOO+ /
<OO/ 0
stringOO0 6
>OO6 7
{OO8 9
$strOO: A
,OOA B
$strOOC K
}OOL M
}OON O
,OOO P
newPP 
(PP 
$strPP 
)PP 
{PP 
PatternsPP #
=PP$ %
newPP& )
ListPP* .
<PP. /
stringPP/ 5
>PP5 6
{PP7 8
$strPP9 @
}PPA B
}PPC D
,PPD E
newQQ 
(QQ 
$strQQ 
)QQ 
{QQ 
PatternsQQ $
=QQ% &
newQQ' *
ListQQ+ /
<QQ/ 0
stringQQ0 6
>QQ6 7
{QQ8 9
$strQQ: B
}QQC D
}QQE F
,QQF G
newRR 
(RR 
$strRR 
)RR 
{RR 
PatternsRR #
=RR$ %
newRR& )
ListRR* .
<RR. /
stringRR/ 5
>RR5 6
{RR7 8
$strRR9 @
}RRA B
}RRC D
,RRD E
newSS 
(SS 
$strSS 
)SS 
{SS 
PatternsSS #
=SS$ %
newSS& )
ListSS* .
<SS. /
stringSS/ 5
>SS5 6
{SS7 8
$strSS9 <
}SS= >
}SS? @
,SS@ A
}TT 
;TT 
publicVV 

voidVV 
ShowContextMenuVV 
(VV  
PixelVV  %
pixelVV& +
)VV+ ,
{WW 
varXX 
manualContextMenuXX 
=XX 
GetContextMenuXX  .
(XX. /
)XX/ 0
;XX0 1
manualContextMenu\\ 
.\\ 
PlacementRect\\ '
=\\( )
new\\* -
(\\- .
pixel\\. 3
.\\3 4
X\\4 5
,\\5 6
pixel\\7 <
.\\< =
Y\\= >
,\\> ?
$num\\@ A
,\\A B
$num\\C D
)\\D E
;\\E F
manualContextMenu^^ 
.^^ 
Open^^ 
(^^ 
ThisControl^^ *
)^^* +
;^^+ ,
}__ 
publicaa 

voidaa 
Resetaa 
(aa 
)aa 
{bb 
Clearcc 
(cc 
)cc 
;cc 
ContextMenuItemsdd 
.dd 
AddRangedd !
(dd! "&
GetDefaultContextMenuItemsdd" <
(dd< =
)dd= >
)dd> ?
;dd? @
}ee 
publicgg 

voidgg 
Cleargg 
(gg 
)gg 
{hh 
ContextMenuItemsii 
.ii 
Clearii 
(ii 
)ii  
;ii  !
}jj 
publicll 

voidll 
Addll 
(ll 
stringll 
Labelll  
,ll  !
Actionll" (
<ll( )
IPlotControlll) 5
>ll5 6
actionll7 =
)ll= >
{mm 
ContextMenuItemsnn 
.nn 
Addnn 
(nn 
newnn  
ContextMenuItemnn! 0
(nn0 1
)nn1 2
{nn3 4
Labelnn5 :
=nn; <
Labelnn= B
,nnB C
OnInvokennD L
=nnM N
actionnnO U
}nnV W
)nnW X
;nnX Y
}oo 
publicqq 

voidqq 
AddSeparatorqq 
(qq 
)qq 
{rr 
ContextMenuItemsss 
.ss 
Addss 
(ss 
newss  
ContextMenuItemss! 0
(ss0 1
)ss1 2
{ss3 4
IsSeparatorss5 @
=ssA B
truessC G
}ssH I
)ssI J
;ssJ K
}tt 
}uu Î
jC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Avalonia\AvaPlotExtensions.cs
	namespace 	
	ScottPlot
 
. 
Avalonia 
; 
internal 
static	 
class 
AvaPlotExtensions '
{ 
internal 
static 
Pixel 
ToPixel !
(! "
this" &
PointerEventArgs' 7
e8 9
,9 :
Visual; A
visualB H
)H I
{ 
float 
x 
= 
( 
float 
) 
e 
. 
GetPosition &
(& '
visual' -
)- .
.. /
X/ 0
;0 1
float 
y 
= 
( 
float 
) 
e 
. 
GetPosition &
(& '
visual' -
)- .
.. /
Y/ 0
;0 1
return 
new 
Pixel 
( 
x 
, 
y 
) 
; 
} 
internal 
static 
Key 
ToKey 
( 
this "
KeyEventArgs# /
e0 1
)1 2
{ 
return 
e 
. 
Key 
switch 
{ 	
AvaKey 
. 
LeftAlt 
=> 
Key !
.! "
Alt" %
,% &
AvaKey 
. 
RightAlt 
=> 
Key "
." #
Alt# &
,& '
AvaKey   
.   
	LeftShift   
=>   
Key    #
.  # $
Shift  $ )
,  ) *
AvaKey!! 
.!! 

RightShift!! 
=>!!  
Key!!! $
.!!$ %
Shift!!% *
,!!* +
AvaKey"" 
."" 
LeftCtrl"" 
=>"" 
Key"" "
.""" #
Ctrl""# '
,""' (
AvaKey## 
.## 
	RightCtrl## 
=>## 
Key##  #
.### $
Ctrl##$ (
,##( )
_$$ 
=>$$ 
Key$$ 
.$$ 
Unknown$$ 
,$$ 
}%% 	
;%%	 

}&& 
internal(( 
static(( 
MouseButton(( 
ToButton((  (
(((( )
this(() -
PointerUpdateKind((. ?
kind((@ D
)((D E
{)) 
return** 
kind** 
switch** 
{++ 	
PointerUpdateKind,, 
.,, 
LeftButtonPressed,, /
=>,,0 2
MouseButton,,3 >
.,,> ?
Left,,? C
,,,C D
PointerUpdateKind-- 
.-- 
LeftButtonReleased-- 0
=>--1 3
MouseButton--4 ?
.--? @
Left--@ D
,--D E
PointerUpdateKind// 
.// 
RightButtonPressed// 0
=>//1 3
MouseButton//4 ?
.//? @
Right//@ E
,//E F
PointerUpdateKind00 
.00 
RightButtonReleased00 1
=>002 4
MouseButton005 @
.00@ A
Right00A F
,00F G
PointerUpdateKind22 
.22 
MiddleButtonPressed22 1
=>222 4
MouseButton225 @
.22@ A
Middle22A G
,22G H
PointerUpdateKind33 
.33  
MiddleButtonReleased33 2
=>333 5
MouseButton336 A
.33A B
Middle33B H
,33H I
_55 
=>55 
MouseButton55 
.55 
Unknown55 $
,55$ %
}66 	
;66	 

}77 
}88 ÐM
`C:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Avalonia\AvaPlot.cs
	namespace 	
	ScottPlot
 
. 
Avalonia 
; 
public 
class 
AvaPlot 
: 
Controls 
.  
Control  '
,' (
IPlotControl) 5
{ 
public 

Plot 
Plot 
{ 
get 
; 
} 
= 
new  #
(# $
)$ %
;% &
public 

IPlotInteraction 
Interaction '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 

	IPlotMenu 
Menu 
{ 
get 
;  
set! $
;$ %
}& '
public 

	GRContext 
? 
	GRContext 
=>  "
null# '
;' (
public 

float 
DisplayScale 
{ 
get  #
;# $
set% (
;( )
}* +
public 

AvaPlot 
( 
) 
{ 
ClipToBounds 
= 
true 
; 
DisplayScale 
= 
DetectDisplayScale )
() *
)* +
;+ ,
Interaction 
= 
new 
Interaction %
(% &
this& *
)* +
;+ ,
Menu 
= 
new 
AvaPlotMenu 
( 
this #
)# $
;$ %
Refresh   
(   
)   
;   
}!! 
private## 
class## 
CustomDrawOp## 
:##   
ICustomDrawOperation##! 5
{$$ 
private%% 
readonly%% 
Plot%% 
_plot%% #
;%%# $
public'' 
Rect'' 
Bounds'' 
{'' 
get''  
;''  !
}''" #
public(( 
bool(( 
HitTest(( 
((( 
Point(( !
p((" #
)((# $
=>((% '
true((( ,
;((, -
public)) 
bool)) 
Equals)) 
())  
ICustomDrawOperation)) /
?))/ 0
other))1 6
)))6 7
=>))8 :
false)); @
;))@ A
public++ 
CustomDrawOp++ 
(++ 
Rect++  
bounds++! '
,++' (
Plot++) -
plot++. 2
)++2 3
{,, 	
_plot-- 
=-- 
plot-- 
;-- 
Bounds.. 
=.. 
bounds.. 
;.. 
}// 	
public11 
void11 
Dispose11 
(11 
)11 
{22 	
}44 	
public66 
void66 
Render66 
(66 #
ImmediateDrawingContext66 2
context663 :
)66: ;
{77 	
var88 
leaseFeature88 
=88 
context88 &
.88& '
TryGetFeature88' 4
<884 5%
ISkiaSharpApiLeaseFeature885 N
>88N O
(88O P
)88P Q
;88Q R
if99 
(99 
leaseFeature99 
is99 
null99  $
)99$ %
return99& ,
;99, -
using;; 
var;; 
lease;; 
=;; 
leaseFeature;; *
.;;* +
Lease;;+ 0
(;;0 1
);;1 2
;;;2 3
	PixelRect<< 
rect<< 
=<< 
new<<  
(<<  !
$num<<! "
,<<" #
(<<$ %
float<<% *
)<<* +
Bounds<<+ 1
.<<1 2
Width<<2 7
,<<7 8
(<<9 :
float<<: ?
)<<? @
Bounds<<@ F
.<<F G
Height<<G M
,<<M N
$num<<O P
)<<P Q
;<<Q R
_plot== 
.== 
Render== 
(== 
lease== 
.== 
SkCanvas== '
,==' (
rect==) -
)==- .
;==. /
}>> 	
}?? 
publicAA 

overrideAA 
voidAA 
RenderAA 
(AA  
DrawingContextAA  .
contextAA/ 6
)AA6 7
{BB 
RectCC 
controlBoundsCC 
=CC 
newCC  
(CC  !
BoundsCC! '
.CC' (
SizeCC( ,
)CC, -
;CC- .
CustomDrawOpDD 
customDrawOpDD !
=DD" #
newDD$ '
(DD' (
controlBoundsDD( 5
,DD5 6
PlotDD7 ;
)DD; <
;DD< =
contextEE 
.EE 
CustomEE 
(EE 
customDrawOpEE #
)EE# $
;EE$ %
}FF 
publicHH 

voidHH 
RefreshHH 
(HH 
)HH 
{II 

DispatcherJJ 
.JJ 
UIThreadJJ 
.JJ 
InvokeAsyncJJ '
(JJ' (
InvalidateVisualJJ( 8
,JJ8 9
DispatcherPriorityJJ: L
.JJL M

BackgroundJJM W
)JJW X
;JJX Y
}KK 
publicMM 

voidMM 
ShowContextMenuMM 
(MM  
PixelMM  %
positionMM& .
)MM. /
{NN 
MenuOO 
.OO 
ShowContextMenuOO 
(OO 
positionOO %
)OO% &
;OO& '
}PP 
	protectedRR 
overrideRR 
voidRR 
OnPointerPressedRR ,
(RR, -#
PointerPressedEventArgsRR- D
eRRE F
)RRF G
{SS 
InteractionTT 
.TT 
	MouseDownTT 
(TT 
positionUU 
:UU 
eUU 
.UU 
ToPixelUU 
(UU  
thisUU  $
)UU$ %
,UU% &
buttonVV 
:VV 
eVV 
.VV 
GetCurrentPointVV %
(VV% &
thisVV& *
)VV* +
.VV+ ,

PropertiesVV, 6
.VV6 7
PointerUpdateKindVV7 H
.VVH I
ToButtonVVI Q
(VVQ R
)VVR S
)VVS T
;VVT U
eXX 	
.XX	 

PointerXX
 
.XX 
CaptureXX 
(XX 
thisXX 
)XX 
;XX  
ifZZ 

(ZZ 
eZZ 
.ZZ 

ClickCountZZ 
==ZZ 
$numZZ 
)ZZ 
{[[ 	
Interaction\\ 
.\\ 
DoubleClick\\ #
(\\# $
)\\$ %
;\\% &
}]] 	
}^^ 
	protected`` 
override`` 
void`` 
OnPointerReleased`` -
(``- .$
PointerReleasedEventArgs``. F
e``G H
)``H I
{aa 
Interactionbb 
.bb 
MouseUpbb 
(bb 
positioncc 
:cc 
ecc 
.cc 
ToPixelcc 
(cc  
thiscc  $
)cc$ %
,cc% &
buttondd 
:dd 
edd 
.dd 
GetCurrentPointdd %
(dd% &
thisdd& *
)dd* +
.dd+ ,

Propertiesdd, 6
.dd6 7
PointerUpdateKinddd7 H
.ddH I
ToButtonddI Q
(ddQ R
)ddR S
)ddS T
;ddT U
eff 	
.ff	 

Pointerff
 
.ff 
Captureff 
(ff 
nullff 
)ff 
;ff  
}gg 
	protectedii 
overrideii 
voidii 
OnPointerMovedii *
(ii* +
PointerEventArgsii+ ;
eii< =
)ii= >
{jj 
Interactionkk 
.kk 
OnMouseMovekk 
(kk  
ekk  !
.kk! "
ToPixelkk" )
(kk) *
thiskk* .
)kk. /
)kk/ 0
;kk0 1
}ll 
	protectednn 
overridenn 
voidnn !
OnPointerWheelChangednn 1
(nn1 2!
PointerWheelEventArgsnn2 G
ennH I
)nnI J
{oo 
floatpp 
deltapp 
=pp 
(pp 
floatpp 
)pp 
epp 
.pp 
Deltapp $
.pp$ %
Ypp% &
;pp& '
ifrr 

(rr 
deltarr 
!=rr 
$numrr 
)rr 
{ss 	
Interactiontt 
.tt 
MouseWheelVerticaltt *
(tt* +
ett+ ,
.tt, -
ToPixeltt- 4
(tt4 5
thistt5 9
)tt9 :
,tt: ;
deltatt< A
)ttA B
;ttB C
}uu 	
}vv 
	protectedxx 
overridexx 
voidxx 
	OnKeyDownxx %
(xx% &
KeyEventArgsxx& 2
exx3 4
)xx4 5
{yy 
Interactionzz 
.zz 
KeyDownzz 
(zz 
ezz 
.zz 
ToKeyzz #
(zz# $
)zz$ %
)zz% &
;zz& '
}{{ 
	protected}} 
override}} 
void}} 
OnKeyUp}} #
(}}# $
KeyEventArgs}}$ 0
e}}1 2
)}}2 3
{~~ 
Interaction 
. 
KeyUp 
( 
e 
. 
ToKey !
(! "
)" #
)# $
;$ %
}
€€ 
public
‚‚ 

float
‚‚  
DetectDisplayScale
‚‚ #
(
‚‚# $
)
‚‚$ %
{
ƒƒ 
return
†† 
$num
†† 
;
†† 
}
‡‡ 
}ˆˆ ã
eC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Avalonia\AssemblyInfo.cs
[ 
assembly 	
:	 

System 
. 
Runtime 
. 
InteropServices )
.) *

ComVisible* 4
(4 5
true5 9
)9 :
]: ;