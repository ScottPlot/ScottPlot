”
gC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Blazor\ExampleJsInterop.cs
	namespace 	
	ScottPlot
 
. 
Blazor 
{ 
public 

class 
ExampleJsInterop !
:" #
IAsyncDisposable$ 4
{ 
private 
readonly 
Lazy 
< 
Task "
<" #
IJSObjectReference# 5
>5 6
>6 7

moduleTask8 B
;B C
public 
ExampleJsInterop 
(  

IJSRuntime  *
	jsRuntime+ 4
)4 5
{ 	

moduleTask 
= 
new 
( 
( 
) 
=>  "
	jsRuntime# ,
., -
InvokeAsync- 8
<8 9
IJSObjectReference9 K
>K L
(L M
$str 
, 
$str K
)K L
.L M
AsTaskM S
(S T
)T U
)U V
;V W
} 	
public 
async 
	ValueTask 
< 
string %
>% &
Prompt' -
(- .
string. 4
message5 <
)< =
{ 	
var 
module 
= 
await 

moduleTask )
.) *
Value* /
;/ 0
return 
await 
module 
.  
InvokeAsync  +
<+ ,
string, 2
>2 3
(3 4
$str4 @
,@ A
messageB I
)I J
;J K
} 	
public 
async 
	ValueTask 
DisposeAsync +
(+ ,
), -
{ 	
if 
( 

moduleTask 
. 
IsValueCreated )
)) *
{ 
var   
module   
=   
await   "

moduleTask  # -
.  - .
Value  . 3
;  3 4
await!! 
module!! 
.!! 
DisposeAsync!! )
(!!) *
)!!* +
;!!+ ,
}"" 
}## 	
}$$ 
}%% Ú
eC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Blazor\BlazorPlotMenu.cs
	namespace 	
	ScottPlot
 
. 
Blazor 
; 
public 
class 
BlazorPlotMenu 
: 
	IPlotMenu '
{ 
public 

void 
ShowContextMenu 
(  
Pixel  %
pixel& +
)+ ,
{ 
} 
public

 

void

 
Reset

 
(

 
)

 
{ 
} 
public 

void 
Clear 
( 
) 
{ 
} 
public 

void 
Add 
( 
string 
Label  
,  !
Action" (
<( )
IPlotControl) 5
>5 6
action7 =
)= >
{ 
} 
public 

void 
AddSeparator 
( 
) 
{ 
} 
} ²p
eC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.Blazor\BlazorPlotBase.cs
	namespace 	
	ScottPlot
 
. 
Blazor 
; 
public 
abstract 
class 
BlazorPlotBase $
:% &
ComponentBase' 4
,4 5
IPlotControl6 B
{		 
[

 
	Parameter

 
]

 
public 

string 
Style 
{ 
get 
; 
set "
;" #
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
[ 
	Parameter 
] 
public 

bool 
EnableRenderLoop  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
false1 6
;6 7
public 

Plot 
Plot 
{ 
get 
; 
private #
set$ '
;' (
}) *
=+ ,
new- 0
(0 1
)1 2
;2 3
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

BlazorPlotBase 
( 
) 
{ 
HandlerPointerMoved 
+= 
OnPointerMoved -
;- .!
HandlerPointerPressed 
+=  
OnPointerPressed! 1
;1 2"
HandlerPointerReleased 
+= !
OnPointerReleased" 3
;3 4
HandlerDoubleTapped 
+= 
OnDoubleTapped -
;- .&
HandlerPointerWheelChanged "
+=# %!
OnPointerWheelChanged& ;
;; <
HandlerKeyDown 
+= 
	OnKeyDown #
;# $
HandlerKeyUp 
+= 
OnKeyUp 
;  
DisplayScale   
=   
DetectDisplayScale   )
(  ) *
)  * +
;  + ,
Interaction!! 
=!! 
new!! 
Interaction!! %
(!!% &
this!!& *
)!!* +
;!!+ ,
Menu"" 
="" 
new"" 
BlazorPlotMenu"" !
(""! "
)""" #
;""# $
}## 
public%% 

	GRContext%% 
?%% 
	GRContext%% 
=>%%  "
null%%# '
;%%' (
public'' 

float'' 
DisplayScale'' 
{'' 
get''  #
;''# $
set''% (
;''( )
}''* +
public)) 

float)) 
DetectDisplayScale)) #
())# $
)))$ %
{** 
return-- 
$num-- 
;-- 
}.. 
public00 

virtual00 
void00 
Refresh00 
(00  
)00  !
{00" #
}00$ %
public22 

Plot22 
Reset22 
(22 
)22 
{33 
Plot44 
newPlot44 
=44 
new44 
(44 
)44 
;44 
Plot55 
oldPlot55 
=55 
Plot55 
;55 
Plot66 
=66 
newPlot66 
;66 
oldPlot77 
?77 
.77 
Dispose77 
(77 
)77 
;77 
return88 
newPlot88 
;88 
}99 
public;; 

void;; 
ShowContextMenu;; 
(;;  
Pixel;;  %
position;;& .
);;. /
{<< 
Menu== 
.== 
ShowContextMenu== 
(== 
position== %
)==% &
;==& '
}>> 
public@@ 

event@@ 
EventHandler@@ 
<@@ 
PointerEventArgs@@ .
>@@. /
HandlerPointerMoved@@0 C
;@@C D
privateBB 
voidBB 
OnPointerMovedBB 
(BB  
objectBB  &
?BB& '
senderBB( .
,BB. /
PointerEventArgsBB0 @
eBBA B
)BBB C
{CC 
InteractionDD 
.DD 
OnMouseMoveDD 
(DD  
CoordinateToPixelDD  1
(DD1 2
eDD2 3
)DD3 4
)DD4 5
;DD5 6
}EE 
publicGG 

voidGG 
OnPointerMovedGG 
(GG 
PointerEventArgsGG /
eGG0 1
)GG1 2
{HH 
HandlerPointerMovedII 
.II 
InvokeII "
(II" #
thisII# '
,II' (
eII) *
)II* +
;II+ ,
}JJ 
publicLL 

eventLL 
EventHandlerLL 
<LL 
PointerEventArgsLL .
>LL. /!
HandlerPointerPressedLL0 E
;LLE F
privateNN 
voidNN 
OnPointerPressedNN !
(NN! "
objectNN" (
?NN( )
senderNN* 0
,NN0 1
PointerEventArgsNN2 B
eNNC D
)NND E
{OO 
InteractionPP 
.PP 
	MouseDownPP 
(PP 
CoordinateToPixelPP /
(PP/ 0
ePP0 1
)PP1 2
,PP2 3!
MapToScottMouseButtonPP4 I
(PPI J
ePPJ K
)PPK L
)PPL M
;PPM N
}QQ 
publicSS 

voidSS 
OnPointerPressedSS  
(SS  !
PointerEventArgsSS! 1
eSS2 3
)SS3 4
{TT !
HandlerPointerPressedUU 
.UU 
InvokeUU $
(UU$ %
thisUU% )
,UU) *
eUU+ ,
)UU, -
;UU- .
}VV 
publicXX 

eventXX 
EventHandlerXX 
<XX 
PointerEventArgsXX .
>XX. /"
HandlerPointerReleasedXX0 F
;XXF G
privateZZ 
voidZZ 
OnPointerReleasedZZ "
(ZZ" #
objectZZ# )
?ZZ) *
senderZZ+ 1
,ZZ1 2
PointerEventArgsZZ3 C
eZZD E
)ZZE F
{[[ 
Interaction\\ 
.\\ 
MouseUp\\ 
(\\ 
CoordinateToPixel\\ -
(\\- .
e\\. /
)\\/ 0
,\\0 1!
MapToScottMouseButton\\2 G
(\\G H
e\\H I
)\\I J
)\\J K
;\\K L
}]] 
public__ 

void__ 
OnPointerReleased__ !
(__! "
PointerEventArgs__" 2
e__3 4
)__4 5
{`` "
HandlerPointerReleasedaa 
.aa 
Invokeaa %
(aa% &
thisaa& *
,aa* +
eaa, -
)aa- .
;aa. /
}bb 
publicdd 

eventdd 
EventHandlerdd 
<dd 
MouseEventArgsdd ,
>dd, -
HandlerDoubleTappeddd. A
;ddA B
privateff 
voidff 
OnDoubleTappedff 
(ff  
objectff  &
?ff& '
senderff( .
,ff. /
MouseEventArgsff0 >
eff? @
)ff@ A
{gg 
Interactionhh 
.hh 
DoubleClickhh 
(hh  
)hh  !
;hh! "
}ii 
publicjj 

voidjj 
OnDoubleTappedjj 
(jj 
MouseEventArgsjj -
ejj. /
)jj/ 0
{kk 
HandlerDoubleTappedll 
.ll 
Invokell "
(ll" #
thisll# '
,ll' (
ell) *
)ll* +
;ll+ ,
}mm 
publicoo 

eventoo 
EventHandleroo 
<oo 
WheelEventArgsoo ,
>oo, -&
HandlerPointerWheelChangedoo. H
;ooH I
publicqq 

voidqq !
OnPointerWheelChangedqq %
(qq% &
objectqq& ,
?qq, -
senderqq. 4
,qq4 5
WheelEventArgsqq6 D
eqqE F
)qqF G
{rr 
Interactionss 
.ss 
MouseWheelVerticalss &
(ss& '
CoordinateToPixelss' 8
(ss8 9
ess9 :
)ss: ;
,ss; <
-ss= >
(ss> ?
floatss? D
)ssD E
essE F
.ssF G
DeltaYssG M
)ssM N
;ssN O
}tt 
publicvv 

voidvv !
OnPointerWheelChangedvv %
(vv% &
WheelEventArgsvv& 4
evv5 6
)vv6 7
{ww &
HandlerPointerWheelChangedxx "
.xx" #
Invokexx# )
(xx) *
thisxx* .
,xx. /
exx0 1
)xx1 2
;xx2 3
}yy 
public|| 

event|| 
EventHandler|| 
<|| 
KeyboardEventArgs|| /
>||/ 0
HandlerKeyDown||1 ?
;||? @
public~~ 

void~~ 
	OnKeyDown~~ 
(~~ 
object~~  
?~~  !
sender~~" (
,~~( )
KeyboardEventArgs~~* ;
e~~< =
)~~= >
{ 
Interaction
€€ 
.
€€ 
KeyDown
€€ 
(
€€ 
MapToScottKey
€€ )
(
€€) *
e
€€* +
.
€€+ ,
Key
€€, /
)
€€/ 0
)
€€0 1
;
€€1 2
}
‚‚ 
public
„„ 

void
„„ 
	OnKeyDown
„„ 
(
„„ 
KeyboardEventArgs
„„ +
e
„„, -
)
„„- .
{
…… 
HandlerKeyDown
†† 
.
†† 
Invoke
†† 
(
†† 
this
†† "
,
††" #
e
††$ %
)
††% &
;
††& '
}
‡‡ 
public
‰‰ 

event
‰‰ 
EventHandler
‰‰ 
<
‰‰ 
KeyboardEventArgs
‰‰ /
>
‰‰/ 0
HandlerKeyUp
‰‰1 =
;
‰‰= >
public
‹‹ 

void
‹‹ 
OnKeyUp
‹‹ 
(
‹‹ 
object
‹‹ 
?
‹‹ 
sender
‹‹  &
,
‹‹& '
KeyboardEventArgs
‹‹( 9
e
‹‹: ;
)
‹‹; <
{
ŒŒ 
Interaction
 
.
 
KeyUp
 
(
 
MapToScottKey
 '
(
' (
e
( )
.
) *
Key
* -
)
- .
)
. /
;
/ 0
}
ŽŽ 
public
 

void
 
OnKeyUp
 
(
 
KeyboardEventArgs
 )
e
* +
)
+ ,
{
‘‘ 
HandlerKeyUp
’’ 
.
’’ 
Invoke
’’ 
(
’’ 
this
’’  
,
’’  !
e
’’" #
)
’’# $
;
’’$ %
}
““ 
public
•• 

Pixel
•• 
CoordinateToPixel
•• "
(
••" #
WheelEventArgs
••# 1
args
••2 6
)
••6 7
{
–– 
return
—— 
new
—— 
Pixel
—— 
(
—— 
(
—— 
float
—— 
)
——  
args
——  $
.
——$ %
OffsetX
——% ,
,
——, -
(
——. /
float
——/ 4
)
——4 5
args
——5 9
.
——9 :
OffsetY
——: A
)
——A B
;
——B C
}
˜˜ 
public
šš 

Pixel
šš 
CoordinateToPixel
šš "
(
šš" #
PointerEventArgs
šš# 3
args
šš4 8
)
šš8 9
{
›› 
return
œœ 
new
œœ 
Pixel
œœ 
(
œœ 
(
œœ 
float
œœ 
)
œœ  
args
œœ  $
.
œœ$ %
OffsetX
œœ% ,
,
œœ, -
(
œœ. /
float
œœ/ 4
)
œœ4 5
args
œœ5 9
.
œœ9 :
OffsetY
œœ: A
)
œœA B
;
œœB C
}
 
public
ŸŸ 

MouseButton
ŸŸ #
MapToScottMouseButton
ŸŸ ,
(
ŸŸ, -
MouseEventArgs
ŸŸ- ;
args
ŸŸ< @
)
ŸŸ@ A
{
   
if
¡¡ 

(
¡¡ 
args
¡¡ 
.
¡¡ 
Button
¡¡ 
==
¡¡ 
$num
¡¡ 
)
¡¡ 
{
¢¢ 	
return
££ 
MouseButton
££ 
.
££ 
Left
££ #
;
££# $
}
¤¤ 	
else
¥¥ 
if
¥¥ 
(
¥¥ 
args
¥¥ 
.
¥¥ 
Button
¥¥ 
==
¥¥ 
$num
¥¥  !
)
¥¥! "
{
¦¦ 	
return
§§ 
MouseButton
§§ 
.
§§ 
Middle
§§ %
;
§§% &
}
¨¨ 	
else
©© 
if
©© 
(
©© 
args
©© 
.
©© 
Button
©© 
==
©© 
$num
©©  !
)
©©! "
{
ªª 	
return
«« 
MouseButton
«« 
.
«« 
Right
«« $
;
««$ %
}
¬¬ 	
else
­­ 
{
®® 	
return
¯¯ 
MouseButton
¯¯ 
.
¯¯ 
Unknown
¯¯ &
;
¯¯& '
}
°° 	
}
±± 
public
³³ 

static
³³ 
Key
³³ 
MapToScottKey
³³ #
(
³³# $
string
³³$ *
key
³³+ .
)
³³. /
{
´´ 
switch
µµ 
(
µµ 
key
µµ 
)
µµ 
{
¶¶ 	
case
·· 
$str
·· 
:
·· 
return
¸¸ 
Key
¸¸ 
.
¸¸ 
Ctrl
¸¸ 
;
¸¸  
case
¹¹ 
$str
¹¹ 
:
¹¹ 
return
ºº 
Key
ºº 
.
ºº 
Alt
ºº 
;
ºº 
case
»» 
$str
»» 
:
»» 
return
¼¼ 
Key
¼¼ 
.
¼¼ 
Shift
¼¼  
;
¼¼  !
default
½½ 
:
½½ 
return
¾¾ 
Key
¾¾ 
.
¾¾ 
Unknown
¾¾ "
;
¾¾" #
}
¿¿ 	
}
ÀÀ 
}ÁÁ 