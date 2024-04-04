⁄7
qC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\Plottables\ScatterGLCustom.cs
	namespace 	
	ScottPlot
 
. 

Plottables 
; 
public 
class 
ScatterGLCustom 
: 
	ScatterGL (
{ 
private 
IMarkersDrawProgram 
?  
JoinsProgram! -
;- .
public 

ScatterGLCustom 
( 
IScatterSource )
data* .
,. /
IPlotControl0 <
control= D
)D E
:F G
baseH L
(L M
dataM Q
,Q R
controlS Z
)Z [
{ 
} 
	protected 
override 
void 
InitializeGL (
(( )
)) *
{ 
base 
. 
InitializeGL 
( 
) 
; 
LinesProgram 
= 
new 
LinesProgramCustom -
(- .
). /
;/ 0
JoinsProgram 
= 
new #
MarkerFillCircleProgram 2
(2 3
)3 4
;4 5
} 
	protected 
override 
void 
RenderWithOpenGL ,
(, -
	SKSurface- 6
surface7 >
,> ?
	GRContext@ I
contextJ Q
)Q R
{ 
int 
height 
= 
( 
int 
) 
surface !
.! "
Canvas" (
.( )
LocalClipBounds) 8
.8 9
Height9 ?
;? @
context   
.   
Flush   
(   
)   
;   
context!! 
.!! 
ResetContext!! 
(!! 
)!! 
;!! 
if## 

(## 
!##  
GLHasBeenInitialized## !
)##! "
InitializeGL$$ 
($$ 
)$$ 
;$$ 
GL&& 

.&&
 
Viewport&& 
(&& 
x'' 
:'' 
('' 
int'' 
)'' 
Axes'' 
.'' 
DataRect'' !
.''! "
Left''" &
,''& '
y(( 
:(( 
((( 
int(( 
)(( 
((( 
height(( 
-(( 
Axes(( "
.((" #
DataRect((# +
.((+ ,
Bottom((, 2
)((2 3
,((3 4
width)) 
:)) 
()) 
int)) 
))) 
Axes)) 
.)) 
DataRect)) %
.))% &
Width))& +
,))+ ,
height** 
:** 
(** 
int** 
)** 
Axes** 
.** 
DataRect** &
.**& '
Height**' -
)**- .
;**. /
if,, 

(,, 
LinesProgram,, 
is,, 
null,,  
),,  !
throw-- 
new-- "
NullReferenceException-- ,
(--, -
nameof--- 3
(--3 4
LinesProgram--4 @
)--@ A
)--A B
;--B C
LinesProgram// 
.// 
Use// 
(// 
)// 
;// 
LinesProgram00 
.00 
SetTransform00 !
(00! "
CalcTransform00" /
(00/ 0
)000 1
)001 2
;002 3
LinesProgram11 
.11 
SetColor11 
(11 
	LineStyle11 '
.11' (
Color11( -
.11- .
	ToTkColor11. 7
(117 8
)118 9
)119 :
;11: ;
LinesProgram22 
.22 
SetViewPortSize22 $
(22$ %
Axes22% )
.22) *
DataRect22* 2
.222 3
Width223 8
,228 9
Axes22: >
.22> ?
DataRect22? G
.22G H
Height22H N
)22N O
;22O P
LinesProgram33 
.33 
SetLineWidth33 !
(33! "
	LineStyle33" +
.33+ ,
Width33, 1
)331 2
;332 3
GL55 

.55
 
BindVertexArray55 
(55 
VertexArrayObject55 ,
)55, -
;55- .
GL66 

.66
 

DrawArrays66 
(66 
PrimitiveType66 #
.66# $
	LineStrip66$ -
,66- .
$num66/ 0
,660 1
VerticesCount662 ?
)66? @
;66@ A
if99 

(99 
MarkerStyle99 
.99 
Size99 
<99 
	LineStyle99 (
.99( )
Width99) .
||:: 
MarkerStyle:: 
.:: 
Shape:: $
==::% '
MarkerShape::( 3
.::3 4

OpenSquare::4 >
||;; 
MarkerStyle;; 
.;; 
Shape;; $
==;;% '
MarkerShape;;( 3
.;;3 4

OpenCircle;;4 >
||<< 
MarkerStyle<< 
.<< 
Shape<< $
==<<% '
MarkerShape<<( 3
.<<3 4
None<<4 8
)<<8 9
{== 	
if>> 
(>> 
JoinsProgram>> 
is>> 
null>>  $
)>>$ %
throw?? 
new?? "
NullReferenceException?? 0
(??0 1
nameof??1 7
(??7 8
JoinsProgram??8 D
)??D E
)??E F
;??F G
JoinsProgramAA 
.AA 
UseAA 
(AA 
)AA 
;AA 
JoinsProgramBB 
.BB 
SetTransformBB %
(BB% &
CalcTransformBB& 3
(BB3 4
)BB4 5
)BB5 6
;BB6 7
JoinsProgramCC 
.CC 
SetFillColorCC %
(CC% &
	LineStyleCC& /
.CC/ 0
ColorCC0 5
.CC5 6
	ToTkColorCC6 ?
(CC? @
)CC@ A
)CCA B
;CCB C
JoinsProgramDD 
.DD 
SetViewPortSizeDD (
(DD( )
AxesDD) -
.DD- .
DataRectDD. 6
.DD6 7
WidthDD7 <
,DD< =
AxesDD> B
.DDB C
DataRectDDC K
.DDK L
HeightDDL R
)DDR S
;DDS T
JoinsProgramEE 
.EE 
SetMarkerSizeEE &
(EE& '
	LineStyleEE' 0
.EE0 1
WidthEE1 6
)EE6 7
;EE7 8
GLFF 
.FF 
BindVertexArrayFF 
(FF 
VertexArrayObjectFF 0
)FF0 1
;FF1 2
GLGG 
.GG 

DrawArraysGG 
(GG 
PrimitiveTypeGG '
.GG' (
PointsGG( .
,GG. /
$numGG0 1
,GG1 2
VerticesCountGG3 @
)GG@ A
;GGA B
}HH 	
RenderMarkersII 
(II 
)II 
;II 
}JJ 
}KK ˚í
kC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\Plottables\ScatterGL.cs
	namespace 	
	ScottPlot
 
. 

Plottables 
; 
public 
class 
	ScatterGL 
: 
Scatter  
,  !
IPlottableGL" .
{ 
public 

IPlotControl 
PlotControl #
{$ %
get& )
;) *
}+ ,
	protected 
int 
VertexBufferObject $
;$ %
	protected 
int 
VertexArrayObject #
;# $
	protected 
ILinesDrawProgram 
?  
LinesProgram! -
;- .
	protected 
IMarkersDrawProgram !
?! "
MarkerProgram# 0
;0 1
	protected 
double 
[ 
] 
Vertices 
;  
	protected 
readonly 
int 
VerticesCount (
;( )
	protected 
bool  
GLHasBeenInitialized '
=( )
false* /
;/ 0
public 
$
GLFallbackRenderStrategy #
Fallback$ ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <$
GLFallbackRenderStrategy= U
.U V
SoftwareV ^
;^ _
public 

	ScatterGL 
( 
IScatterSource #
data$ (
,( )
IPlotControl* 6
control7 >
)> ?
:@ A
baseB F
(F G
dataG K
)K L
{ 
PlotControl 
= 
control 
; 
var 

dataPoints 
= 
data 
. 
GetScatterPoints .
(. /
)/ 0
;0 1
Vertices 
= 
new 
double 
[ 

dataPoints (
.( )
Count) .
*/ 0
$num1 2
]2 3
;3 4
for   
(   
int   
i   
=   
$num   
;   
i   
<   

dataPoints   &
.  & '
Count  ' ,
;  , -
i  . /
++  / 1
)  1 2
{!! 	
Vertices"" 
["" 
i"" 
*"" 
$num"" 
]"" 
="" 

dataPoints"" (
[""( )
i"") *
]""* +
.""+ ,
X"", -
;""- .
Vertices## 
[## 
i## 
*## 
$num## 
+## 
$num## 
]## 
=##  !

dataPoints##" ,
[##, -
i##- .
]##. /
.##/ 0
Y##0 1
;##1 2
}$$ 	
VerticesCount%% 
=%% 
Vertices%%  
.%%  !
Length%%! '
/%%( )
$num%%* +
;%%+ ,
}&& 
	protected(( 
virtual(( 
void(( 
InitializeGL(( '
(((' (
)((( )
{)) 
LinesProgram** 
=** 
new** 
LinesProgram** '
(**' (
)**( )
;**) *
MarkerProgram++ 
=++ 
new++ #
MarkerFillCircleProgram++ 3
(++3 4
)++4 5
;++5 6
VertexArrayObject-- 
=-- 
GL-- 
.-- 
GenVertexArray-- -
(--- .
)--. /
;--/ 0
VertexBufferObject.. 
=.. 
GL.. 
...  
	GenBuffer..  )
(..) *
)..* +
;..+ ,
GL// 

.//
 
BindVertexArray// 
(// 
VertexArrayObject// ,
)//, -
;//- .
GL00 

.00
 

BindBuffer00 
(00 
BufferTarget00 "
.00" #
ArrayBuffer00# .
,00. /
VertexBufferObject000 B
)00B C
;00C D
GL11 

.11
 

BufferData11 
(11 
BufferTarget11 "
.11" #
ArrayBuffer11# .
,11. /
Vertices110 8
.118 9
Length119 ?
*11@ A
sizeof11B H
(11H I
double11I O
)11O P
,11P Q
Vertices11R Z
,11Z [
BufferUsageHint11\ k
.11k l

StaticDraw11l v
)11v w
;11w x
GL22 

.22
  
VertexAttribLPointer22 
(22  
$num22  !
,22! "
$num22# $
,22$ %"
VertexAttribDoubleType22& <
.22< =
Double22= C
,22C D
$num22E F
,22F G
IntPtr22H N
.22N O
Zero22O S
)22S T
;22T U
GL33 

.33
 #
EnableVertexAttribArray33 "
(33" #
$num33# $
)33$ %
;33% &
Vertices44 
=44 
Array44 
.44 
Empty44 
<44 
double44 %
>44% &
(44& '
)44' (
;44( ) 
GLHasBeenInitialized55 
=55 
true55 #
;55# $
}66 
	protected88 
Matrix4d88 
CalcTransform88 $
(88$ %
)88% &
{99 
var:: 
xRange:: 
=:: 
Axes:: 
.:: 
XAxis:: 
.::  
Range::  %
;::% &
var;; 
yRange;; 
=;; 
Axes;; 
.;; 
YAxis;; 
.;;  
Range;;  %
;;;% &
Matrix4d== 
	translate== 
=== 
Matrix4d== %
.==% &
CreateTranslation==& 7
(==7 8
x>> 
:>> 
->> 
$num>> 
*>> 
(>> 
xRange>> 
.>> 
Min>> !
+>>" #
xRange>>$ *
.>>* +
Max>>+ .
)>>. /
/>>0 1
$num>>2 3
,>>3 4
y?? 
:?? 
-?? 
$num?? 
*?? 
(?? 
yRange?? 
.?? 
Min?? !
+??" #
yRange??$ *
.??* +
Max??+ .
)??. /
/??0 1
$num??2 3
,??3 4
z@@ 
:@@ 
$num@@ 
)@@ 
;@@ 
Matrix4dBB 
scaleBB 
=BB 
Matrix4dBB !
.BB! "
ScaleBB" '
(BB' (
xCC 
:CC 
$numCC 
/CC 
(CC 
xRangeCC 
.CC 
MaxCC  
-CC! "
xRangeCC# )
.CC) *
MinCC* -
)CC- .
,CC. /
yDD 
:DD 
$numDD 
/DD 
(DD 
yRangeDD 
.DD 
MaxDD  
-DD! "
yRangeDD# )
.DD) *
MinDD* -
)DD- .
,DD. /
zEE 
:EE 
$numEE 
)EE 
;EE 
returnGG 
	translateGG 
*GG 
scaleGG  
;GG  !
}HH 
publicJJ 

newJJ 
voidJJ 
RenderJJ 
(JJ 

RenderPackJJ %
rpJJ& (
)JJ( )
{KK 
SystemLL 
.LL 
DiagnosticsLL 
.LL 
DebugLL  
.LL  !
	WriteLineLL! *
(LL* +
$strLL+ c
)LLc d
;LLd e
baseMM 
.MM 
RenderMM 
(MM 
rpMM 
)MM 
;MM 
}NN 
publicPP 

voidPP 
RenderPP 
(PP 
	SKSurfacePP  
surfacePP! (
)PP( )
{QQ 
ifRR 

(RR 
PlotControlRR 
.RR 
	GRContextRR !
isRR" $
notRR% (
nullRR) -
&&RR. 0
surfaceRR1 8
.RR8 9
ContextRR9 @
isRRA C
notRRD G
nullRRH L
)RRL M
{SS 	
RenderWithOpenGLTT 
(TT 
surfaceTT $
,TT$ %
PlotControlTT& 1
.TT1 2
	GRContextTT2 ;
)TT; <
;TT< =
returnUU 
;UU 
}VV 	
ifXX 

(XX 
FallbackXX 
==XX $
GLFallbackRenderStrategyXX 0
.XX0 1
SoftwareXX1 9
)XX9 :
{YY 	
surfaceZZ 
.ZZ 
CanvasZZ 
.ZZ 
ClipRectZZ #
(ZZ# $
AxesZZ$ (
.ZZ( )
DataRectZZ) 1
.ZZ1 2
ToSKRectZZ2 :
(ZZ: ;
)ZZ; <
)ZZ< =
;ZZ= >
	PixelSize[[ 

figureSize[[  
=[[! "
new[[# &
([[& '
surface[[' .
.[[. /
Canvas[[/ 5
.[[5 6
LocalClipBounds[[6 E
.[[E F
Width[[F K
,[[K L
surface[[M T
.[[T U
Canvas[[U [
.[[[ \
LocalClipBounds[[\ k
.[[k l
Height[[l r
)[[r s
;[[s t
	PixelRect\\ 
rect\\ 
=\\ 
new\\  
(\\  !
$num\\! "
,\\" #

figureSize\\$ .
.\\. /
Width\\/ 4
,\\4 5

figureSize\\6 @
.\\@ A
Height\\A G
,\\G H
$num\\I J
)\\J K
;\\K L

RenderPack]] 
rp]] 
=]] 
new]] 
(]]  
PlotControl]]  +
.]]+ ,
Plot]], 0
,]]0 1
rect]]2 6
,]]6 7
surface]]8 ?
.]]? @
Canvas]]@ F
)]]F G
;]]G H
Render^^ 
(^^ 
rp^^ 
)^^ 
;^^ 
}__ 	
}`` 
	protectedbb 
virtualbb 
voidbb 
RenderWithOpenGLbb +
(bb+ ,
	SKSurfacebb, 5
surfacebb6 =
,bb= >
	GRContextbb? H
contextbbI P
)bbP Q
{cc 
intdd 
heightdd 
=dd 
(dd 
intdd 
)dd 
surfacedd !
.dd! "
Canvasdd" (
.dd( )
LocalClipBoundsdd) 8
.dd8 9
Heightdd9 ?
;dd? @
contextff 
.ff 
Flushff 
(ff 
)ff 
;ff 
contextgg 
.gg 
ResetContextgg 
(gg 
)gg 
;gg 
ifii 

(ii 
!ii  
GLHasBeenInitializedii !
)ii! "
InitializeGLjj 
(jj 
)jj 
;jj 
GLll 

.ll
 
Viewportll 
(ll 
xmm 
:mm 
(mm 
intmm 
)mm 
Axesmm 
.mm 
DataRectmm !
.mm! "
Leftmm" &
,mm& '
ynn 
:nn 
(nn 
intnn 
)nn 
(nn 
heightnn 
-nn 
Axesnn "
.nn" #
DataRectnn# +
.nn+ ,
Bottomnn, 2
)nn2 3
,nn3 4
widthoo 
:oo 
(oo 
intoo 
)oo 
Axesoo 
.oo 
DataRectoo %
.oo% &
Widthoo& +
,oo+ ,
heightpp 
:pp 
(pp 
intpp 
)pp 
Axespp 
.pp 
DataRectpp &
.pp& '
Heightpp' -
)pp- .
;pp. /
ifrr 

(rr 
LinesProgramrr 
isrr 
nullrr  
)rr  !
throwss 
newss "
NullReferenceExceptionss ,
(ss, -
nameofss- 3
(ss3 4
LinesProgramss4 @
)ss@ A
)ssA B
;ssB C
LinesProgramuu 
.uu 
Useuu 
(uu 
)uu 
;uu 
LinesProgramvv 
.vv 
SetTransformvv !
(vv! "
CalcTransformvv" /
(vv/ 0
)vv0 1
)vv1 2
;vv2 3
LinesProgramww 
.ww 
SetColorww 
(ww 
	LineStyleww '
.ww' (
Colorww( -
.ww- .
	ToTkColorww. 7
(ww7 8
)ww8 9
)ww9 :
;ww: ;
GLxx 

.xx
 
BindVertexArrayxx 
(xx 
VertexArrayObjectxx ,
)xx, -
;xx- .
GLyy 

.yy
 

DrawArraysyy 
(yy 
PrimitiveTypeyy #
.yy# $
	LineStripyy$ -
,yy- .
$numyy/ 0
,yy0 1
VerticesCountyy2 ?
)yy? @
;yy@ A
RenderMarkers{{ 
({{ 
){{ 
;{{ 
}|| 
	protected~~ 
void~~ 
RenderMarkers~~  
(~~  !
)~~! "
{ 
if
ÄÄ 

(
ÄÄ 
MarkerStyle
ÄÄ 
.
ÄÄ 
Shape
ÄÄ 
==
ÄÄ  
MarkerShape
ÄÄ! ,
.
ÄÄ, -
None
ÄÄ- 1
||
ÄÄ2 4
MarkerStyle
ÄÄ5 @
.
ÄÄ@ A
Size
ÄÄA E
==
ÄÄF H
$num
ÄÄI J
)
ÄÄJ K
return
ÅÅ 
;
ÅÅ !
IMarkersDrawProgram
ÉÉ 
?
ÉÉ 

newProgram
ÉÉ '
=
ÉÉ( )
MarkerStyle
ÉÉ* 5
.
ÉÉ5 6
Shape
ÉÉ6 ;
switch
ÉÉ< B
{
ÑÑ 	
MarkerShape
ÖÖ 
.
ÖÖ 
FilledSquare
ÖÖ $
=>
ÖÖ% '
MarkerProgram
ÖÖ( 5
is
ÖÖ6 8%
MarkerFillSquareProgram
ÖÖ9 P
?
ÖÖQ R
null
ÖÖS W
:
ÖÖX Y
new
ÖÖZ ]%
MarkerFillSquareProgram
ÖÖ^ u
(
ÖÖu v
)
ÖÖv w
,
ÖÖw x
MarkerShape
ÜÜ 
.
ÜÜ 
FilledCircle
ÜÜ $
=>
ÜÜ% '
MarkerProgram
ÜÜ( 5
is
ÜÜ6 8%
MarkerFillCircleProgram
ÜÜ9 P
?
ÜÜQ R
null
ÜÜS W
:
ÜÜX Y
new
ÜÜZ ]%
MarkerFillCircleProgram
ÜÜ^ u
(
ÜÜu v
)
ÜÜv w
,
ÜÜw x
MarkerShape
áá 
.
áá 

OpenCircle
áá "
=>
áá# %
MarkerProgram
áá& 3
is
áá4 6%
MarkerOpenCircleProgram
áá7 N
?
ááO P
null
ááQ U
:
ááV W
new
ááX [%
MarkerOpenCircleProgram
áá\ s
(
áás t
)
áát u
,
ááu v
MarkerShape
àà 
.
àà 

OpenSquare
àà "
=>
àà# %
MarkerProgram
àà& 3
is
àà4 6%
MarkerOpenSquareProgram
àà7 N
?
ààO P
null
ààQ U
:
ààV W
new
ààX [%
MarkerOpenSquareProgram
àà\ s
(
ààs t
)
ààt u
,
ààu v
_
ââ 
=>
ââ 
throw
ââ 
new
ââ #
NotSupportedException
ââ 0
(
ââ0 1
$"
ââ1 3
$str
ââ3 A
{
ââA B
MarkerStyle
ââB M
.
ââM N
Shape
ââN S
}
ââS T
$str
ââT v
"
ââv w
)
ââw x
,
ââx y
}
ää 	
;
ää	 

if
åå 

(
åå 

newProgram
åå 
is
åå 
not
åå 
null
åå "
)
åå" #
{
çç 	
MarkerProgram
éé 
?
éé 
.
éé 
Dispose
éé "
(
éé" #
)
éé# $
;
éé$ %
MarkerProgram
èè 
=
èè 

newProgram
èè &
;
èè& '
}
êê 	
if
íí 

(
íí 
MarkerProgram
íí 
is
íí 
null
íí !
)
íí! "
throw
ìì 
new
ìì $
NullReferenceException
ìì ,
(
ìì, -
nameof
ìì- 3
(
ìì3 4
MarkerProgram
ìì4 A
)
ììA B
)
ììB C
;
ììC D
MarkerProgram
ïï 
.
ïï 
Use
ïï 
(
ïï 
)
ïï 
;
ïï 
MarkerProgram
ññ 
.
ññ 
SetTransform
ññ "
(
ññ" #
CalcTransform
ññ# 0
(
ññ0 1
)
ññ1 2
)
ññ2 3
;
ññ3 4
MarkerProgram
óó 
.
óó 
SetMarkerSize
óó #
(
óó# $
MarkerStyle
óó$ /
.
óó/ 0
Size
óó0 4
)
óó4 5
;
óó5 6
MarkerProgram
òò 
.
òò 
SetFillColor
òò "
(
òò" #
MarkerStyle
òò# .
.
òò. /
Fill
òò/ 3
.
òò3 4
Color
òò4 9
.
òò9 :
	ToTkColor
òò: C
(
òòC D
)
òòD E
)
òòE F
;
òòF G
MarkerProgram
ôô 
.
ôô 
SetViewPortSize
ôô %
(
ôô% &
Axes
ôô& *
.
ôô* +
DataRect
ôô+ 3
.
ôô3 4
Width
ôô4 9
,
ôô9 :
Axes
ôô; ?
.
ôô? @
DataRect
ôô@ H
.
ôôH I
Height
ôôI O
)
ôôO P
;
ôôP Q
MarkerProgram
öö 
.
öö 
SetOutlineColor
öö %
(
öö% &
MarkerStyle
öö& 1
.
öö1 2
Outline
öö2 9
.
öö9 :
Color
öö: ?
.
öö? @
	ToTkColor
öö@ I
(
ööI J
)
ööJ K
)
ööK L
;
ööL M
MarkerProgram
õõ 
.
õõ 
SetOpenFactor
õõ #
(
õõ# $
$num
õõ$ (
-
õõ) *
(
õõ+ ,
float
õõ, 1
)
õõ1 2
MarkerStyle
õõ2 =
.
õõ= >
Outline
õõ> E
.
õõE F
Width
õõF K
*
õõL M
$num
õõN O
/
õõP Q
MarkerStyle
õõR ]
.
õõ] ^
Size
õõ^ b
)
õõb c
;
õõc d
GL
úú 

.
úú
 
BindVertexArray
úú 
(
úú 
VertexArrayObject
úú ,
)
úú, -
;
úú- .
GL
ùù 

.
ùù
 

DrawArrays
ùù 
(
ùù 
PrimitiveType
ùù #
.
ùù# $
Points
ùù$ *
,
ùù* +
$num
ùù, -
,
ùù- .
VerticesCount
ùù/ <
)
ùù< =
;
ùù= >
}
ûû 
public
†† 

void
†† 
GLFinish
†† 
(
†† 
)
†† 
=>
†† 
LinesProgram
†† *
?
††* +
.
††+ ,
GLFinish
††, 4
(
††4 5
)
††5 6
;
††6 7
}°° ™
oC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\MarkerProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
abstract 
class 
MarkersProgram $
:% &
GLProgramBase' 4
,4 5
IMarkersDrawProgram6 I
{ 
	protected		 
override		 
string		 
?		 
VertexShaderSource		 1
=>		2 4
null		5 9
;		9 :
	protected 
override 
string 
?  
GeometryShaderSource 3
=>4 6
null7 ;
;; <
	protected 
override 
string 
?  
FragmentShaderSource 3
=>4 6
null7 ;
;; <
public 

void 
SetTransform 
( 
Matrix4d %
	transform& /
)/ 0
{ 
var 
location 
= 
GetUniformLocation )
() *
$str* 5
)5 6
;6 7
GL 

.
 
UniformMatrix4 
( 
location "
," #
true$ (
,( )
ref* -
	transform. 7
)7 8
;8 9
} 
public 

virtual 
void 
SetFillColor $
($ %
Color4% +
color, 1
)1 2
{ 
var 
location 
= 
GetUniformLocation )
() *
$str* 5
)5 6
;6 7
GL 

.
 
Uniform4 
( 
location 
, 
color #
)# $
;$ %
} 
public 

void 
SetMarkerSize 
( 
float #
size$ (
)( )
{ 
var 
location 
= 
GetUniformLocation )
() *
$str* 7
)7 8
;8 9
GL 

.
 
Uniform1 
( 
location 
, 
size "
)" #
;# $
} 
public!! 

virtual!! 
void!! 
SetOutlineColor!! '
(!!' (
Color4!!( .
color!!/ 4
)!!4 5
{"" 
}$$ 
public&& 

void&& 
SetViewPortSize&& 
(&&  
float&&  %
width&&& +
,&&+ ,
float&&- 2
height&&3 9
)&&9 :
{'' 
int(( 
location(( 
=(( 
GetUniformLocation(( )
((() *
$str((* ;
)((; <
;((< =
Vector2)) 
viewPortSize)) 
=)) 
new)) "
())" #
width))# (
,))( )
height))* 0
)))0 1
;))1 2
GL** 

.**
 
Uniform2** 
(** 
location** 
,** 
viewPortSize** *
)*** +
;**+ ,
}++ 
public-- 

virtual-- 
void-- 
SetOpenFactor-- %
(--% &
float--& +
factor--, 2
)--2 3
{.. 
}00 
}11 ÷
yC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\MarkerOpenSquareProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
class #
MarkerOpenSquareProgram $
:% &#
MarkerFillSquareProgram' >
{ 
	protected 
override 
string  
GeometryShaderSource 2
=>3 5
$str	% 

;%%
 
	protected'' 
override'' 
string''  
FragmentShaderSource'' 2
=>''3 5
$str(4 

;44
 
public66 

override66 
void66 
SetFillColor66 %
(66% &
Color466& ,
color66- 2
)662 3
{77 
}99 
public;; 

override;; 
void;; 
SetOpenFactor;; &
(;;& '
float;;' ,

openFactor;;- 7
);;7 8
{<< 
var== 
location== 
=== 
GetUniformLocation== )
(==) *
$str==* 6
)==6 7
;==7 8
GL>> 

.>>
 
Uniform1>> 
(>> 
location>> 
,>> 

openFactor>> (
)>>( )
;>>) *
}?? 
publicAA 

overrideAA 
voidAA 
SetOutlineColorAA (
(AA( )
Color4AA) /
colorAA0 5
)AA5 6
{BB 
varCC 
locationCC 
=CC 
GetUniformLocationCC )
(CC) *
$strCC* 5
)CC5 6
;CC6 7
GLDD 

.DD
 
Uniform4DD 
(DD 
locationDD 
,DD 
colorDD #
)DD# $
;DD$ %
}EE 
}FF ¬
yC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\MarkerOpenCircleProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
class #
MarkerOpenCircleProgram $
:% &#
MarkerFillCircleProgram' >
{ 
	protected 
override 
string  
FragmentShaderSource 2
=>3 5
$str	 

;
 
public 

override 
void 
SetFillColor %
(% &
Color4& ,
color- 2
)2 3
{ 
} 
public 

override 
void 
SetOpenFactor &
(& '
float' ,

openFactor- 7
)7 8
{ 
var 
location 
= 
GetUniformLocation )
() *
$str* 6
)6 7
;7 8
GL 

.
 
Uniform1 
( 
location 
, 

openFactor (
)( )
;) *
} 
public!! 

override!! 
void!! 
SetOutlineColor!! (
(!!( )
Color4!!) /
color!!0 5
)!!5 6
{"" 
var## 
location## 
=## 
GetUniformLocation## )
(##) *
$str##* 5
)##5 6
;##6 7
GL$$ 

.$$
 
Uniform4$$ 
($$ 
location$$ 
,$$ 
color$$ #
)$$# $
;$$$ %
}%% 
}&& …
yC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\MarkerFillSquareProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
class #
MarkerFillSquareProgram $
:% &
MarkersProgram' 5
{ 
	protected 
override 
string 
VertexShaderSource 0
=>1 3
$str 

;
 
	protected 
override 
string  
GeometryShaderSource 2
=>3 5
$str) 

;))
 
	protected++ 
override++ 
string++  
FragmentShaderSource++ 2
=>++3 5
$str,3 

;33
 
}44 …
yC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\MarkerFillCircleProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
class #
MarkerFillCircleProgram $
:% &
MarkersProgram' 5
{ 
	protected 
override 
string 
VertexShaderSource 0
=>1 3
$str 

;
 
	protected 
override 
string  
GeometryShaderSource 2
=>3 5
$str/ 

;//
 
	protected11 
override11 
string11  
FragmentShaderSource11 2
=>113 5
$str2= 

;==
 
}>> ˇ
tC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\LinesProgramCustom.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public

 
class

 
LinesProgramCustom

 
:

  !
GLProgramBase

" /
,

/ 0
ILinesDrawProgram

1 B
{ 
	protected 
override 
string 
VertexShaderSource 0
=>1 3
$str 

;
 
	protected 
override 
string  
GeometryShaderSource 2
=>3 5
$str= 

;==
 
	protected?? 
override?? 
string??  
FragmentShaderSource?? 2
=>??3 5
$str@I 
;II 
publicKK 

voidKK 
SetLineWidthKK 
(KK 
floatKK "
	lineWidthKK# ,
)KK, -
{LL 
intMM 
locationMM 
=MM 
GetUniformLocationMM )
(MM) *
$strMM* 8
)MM8 9
;MM9 :
GLNN 

.NN
 
Uniform1NN 
(NN 
locationNN 
,NN 
	lineWidthNN '
)NN' (
;NN( )
}OO 
publicQQ 

voidQQ 
SetViewPortSizeQQ 
(QQ  
floatQQ  %
widthQQ& +
,QQ+ ,
floatQQ- 2
heightQQ3 9
)QQ9 :
{RR 
intSS 
locationSS 
=SS 
GetUniformLocationSS )
(SS) *
$strSS* ;
)SS; <
;SS< =
Vector2TT 
viewPortSizeTT 
=TT 
newTT "
Vector2TT# *
(TT* +
widthTT+ 0
,TT0 1
heightTT2 8
)TT8 9
;TT9 :
GLUU 

.UU
 
Uniform2UU 
(UU 
locationUU 
,UU 
viewPortSizeUU *
)UU* +
;UU+ ,
}VV 
publicXX 

voidXX 
SetTransformXX 
(XX 
Matrix4dXX %
	transformXX& /
)XX/ 0
{YY 
varZZ 
locationZZ 
=ZZ 
GetUniformLocationZZ )
(ZZ) *
$strZZ* 5
)ZZ5 6
;ZZ6 7
GL[[ 

.[[
 
UniformMatrix4[[ 
([[ 
location[[ "
,[[" #
true[[$ (
,[[( )
ref[[* -
	transform[[. 7
)[[7 8
;[[8 9
}\\ 
public^^ 

void^^ 
SetColor^^ 
(^^ 
Color4^^ 
color^^  %
)^^% &
{__ 
var`` 
location`` 
=`` 
GetUniformLocation`` )
(``) *
$str``* 5
)``5 6
;``6 7
GLaa 

.aa
 
Uniform4aa 
(aa 
locationaa 
,aa 
coloraa #
)aa# $
;aa$ %
}bb 
}cc ≤
nC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\LinesProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
class 
LinesProgram 
: 
GLProgramBase )
,) *
ILinesDrawProgram+ <
{ 
	protected 
override 
string 
VertexShaderSource 0
=>1 3
$str 
; 	
	protected 
override 
string  
FragmentShaderSource 2
=>3 5
$str! 
;!! 	
public## 

void## 
SetTransform## 
(## 
Matrix4d## %
	transform##& /
)##/ 0
{$$ 
var%% 
location%% 
=%% 
GetUniformLocation%% )
(%%) *
$str%%* 5
)%%5 6
;%%6 7
GL&& 

.&&
 
UniformMatrix4&& 
(&& 
location&& "
,&&" #
true&&$ (
,&&( )
ref&&* -
	transform&&. 7
)&&7 8
;&&8 9
}'' 
public)) 

void)) 
SetColor)) 
()) 
Color4)) 
color))  %
)))% &
{** 
var++ 
location++ 
=++ 
GetUniformLocation++ )
(++) *
$str++* 5
)++5 6
;++6 7
GL,, 

.,,
 
Uniform4,, 
(,, 
location,, 
,,, 
color,, #
),,# $
;,,$ %
}-- 
public// 

void// 
SetLineWidth// 
(// 
float// "
	lineWidth//# ,
)//, -
{00 
throw11 
new11 !
NotSupportedException11 '
(11' (
nameof11( .
(11. /
SetLineWidth11/ ;
)11; <
)11< =
;11= >
}22 
public44 

void44 
SetViewPortSize44 
(44  
float44  %
width44& +
,44+ ,
float44- 2
height443 9
)449 :
{55 
throw66 
new66 !
NotSupportedException66 '
(66' (
nameof66( .
(66. /
SetViewPortSize66/ >
)66> ?
)66? @
;66@ A
}77 
}88 »	
uC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\IMarkersDrawProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
	interface 
IMarkersDrawProgram $
:% &

IGLProgram' 1
{ 
void 
SetFillColor	 
( 
Color4 
color "
)" #
;# $
void		 
SetOutlineColor			 
(		 
Color4		 
color		  %
)		% &
;		& '
void

 
SetMarkerSize

	 
(

 
float

 
size

 !
)

! "
;

" #
void 
SetTransform	 
( 
Matrix4d 
	transform (
)( )
;) *
void 
SetViewPortSize	 
( 
float 
width $
,$ %
float& +
height, 2
)2 3
;3 4
void 
SetOpenFactor	 
( 
float 
factor #
)# $
;$ %
} ¿
sC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\ILinesDrawProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
	interface 
ILinesDrawProgram "
:# $

IGLProgram% /
{ 
void 
SetLineWidth	 
( 
float 
	lineWidth %
)% &
;& '
void		 
SetViewPortSize			 
(		 
float		 
width		 $
,		$ %
float		& +
height		, 2
)		2 3
;		3 4
void

 
SetTransform

	 
(

 
Matrix4d

 
	transform

 (
)

( )
;

) *
void 
SetColor	 
( 
Color4 
color 
) 
;  
} ¢
lC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\IGLProgram.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
	interface 

IGLProgram 
: 
IDisposable )
{ 
void 
Use	 
( 
) 
; 
int 
GetUniformLocation 
( 
string !
name" &
)& '
;' (
int		 
GetAttribLocation		 
(		 
string		  
name		! %
)		% &
;		& '
void

 
GLFinish

	 
(

 
)

 
;

 
} Œ
jC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\GLShader.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
class 
GLShader 
{ 
private 
readonly 
int 
Handle 
;  
private		 
bool		 
active		 
=		 
false		 
;		  
public 

GLShader 
( 

ShaderType 
type #
,# $
string% +
?+ ,

sourceCode- 7
)7 8
{ 
if 

( 

sourceCode 
is 
null 
) 
return 
; 
Handle 
= 
GL 
. 
CreateShader  
(  !
type! %
)% &
;& '
active 
= 
true 
; 
Compile 
( 

sourceCode 
) 
; 
} 
public 

void 
AttachToProgram 
(  
int  #
programHandle$ 1
)1 2
{ 
if 

( 
! 
active 
) 
return 
; 
GL 

.
 
AttachShader 
( 
programHandle %
,% &
Handle' -
)- .
;. /
} 
public 

void 
DetachFromProgram !
(! "
int" %
programHandle& 3
)3 4
{ 
if 

( 
! 
active 
) 
return   
;   
GL"" 

.""
 
DetachShader"" 
("" 
programHandle"" %
,""% &
Handle""' -
)""- .
;"". /
GL## 

.##
 
DeleteShader## 
(## 
Handle## 
)## 
;##  
active$$ 
=$$ 
false$$ 
;$$ 
}%% 
private'' 
void'' 
Compile'' 
('' 
string'' 

sourceCode''  *
)''* +
{(( 
GL)) 

.))
 
ShaderSource)) 
()) 
Handle)) 
,)) 

sourceCode))  *
)))* +
;))+ ,
GL** 

.**
 
CompileShader** 
(** 
Handle** 
)**  
;**  !
GL++ 

.++
 
	GetShader++ 
(++ 
Handle++ 
,++ 
ShaderParameter++ ,
.++, -
CompileStatus++- :
,++: ;
out++< ?
int++@ C
successVertex++D Q
)++Q R
;++R S
if,, 

(,, 
successVertex,, 
==,, 
$num,, 
),, 
{-- 	
string.. 
infoLog.. 
=.. 
GL.. 
...  
GetShaderInfoLog..  0
(..0 1
Handle..1 7
)..7 8
;..8 9
Debug// 
.// 
	WriteLine// 
(// 
infoLog// #
)//# $
;//$ %
active00 
=00 
false00 
;00 
}11 	
}22 
}33 ˚,
oC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLPrograms\GLProgramBase.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
. 

GLPrograms %
;% &
public 
abstract 
class 
GLProgramBase #
:$ %

IGLProgram& 0
{ 
private		 
readonly		 
int		 
Handle		 
;		  
	protected 
virtual 
string 
? 
VertexShaderSource 0
=>1 3
null4 8
;8 9
	protected 
virtual 
string 
?  
GeometryShaderSource 2
=>3 5
null6 :
;: ;
	protected 
virtual 
string 
?  
FragmentShaderSource 2
=>3 5
null6 :
;: ;
public 

GLProgramBase 
( 
) 
{ 
var 
VertexShader 
= 
new 
GLShader '
(' (

ShaderType( 2
.2 3
VertexShader3 ?
,? @
VertexShaderSourceA S
)S T
;T U
var 
GeometryShader 
= 
new  
GLShader! )
() *

ShaderType* 4
.4 5
GeometryShader5 C
,C D 
GeometryShaderSourceE Y
)Y Z
;Z [
var 
FragmentShader 
= 
new  
GLShader! )
() *

ShaderType* 4
.4 5
FragmentShader5 C
,C D 
FragmentShaderSourceE Y
)Y Z
;Z [
Handle 
= 
GL 
. 
CreateProgram !
(! "
)" #
;# $
VertexShader 
. 
AttachToProgram $
($ %
Handle% +
)+ ,
;, -
GeometryShader 
. 
AttachToProgram &
(& '
Handle' -
)- .
;. /
FragmentShader 
. 
AttachToProgram &
(& '
Handle' -
)- .
;. /
GL 

.
 
LinkProgram 
( 
Handle 
) 
; 
GL 

.
 

GetProgram 
( 
Handle 
, #
GetProgramParameterName 5
.5 6

LinkStatus6 @
,@ A
outB E
intF I
successJ Q
)Q R
;R S
if 

( 
success 
== 
$num 
) 
{   	
string!! 
infoLog!! 
=!! 
GL!! 
.!!  
GetProgramInfoLog!!  1
(!!1 2
Handle!!2 8
)!!8 9
;!!9 :
Debug"" 
."" 
	WriteLine"" 
("" 
infoLog"" #
)""# $
;""$ %
}## 	
VertexShader%% 
.%% 
DetachFromProgram%% &
(%%& '
Handle%%' -
)%%- .
;%%. /
GeometryShader&& 
.&& 
DetachFromProgram&& (
(&&( )
Handle&&) /
)&&/ 0
;&&0 1
FragmentShader'' 
.'' 
DetachFromProgram'' (
(''( )
Handle'') /
)''/ 0
;''0 1
}(( 
public** 

void** 
Use** 
(** 
)** 
{++ 
GL,, 

.,,
 

UseProgram,, 
(,, 
Handle,, 
),, 
;,, 
}-- 
public// 

int// 
GetAttribLocation//  
(//  !
string//! '

attribName//( 2
)//2 3
{00 
return11 
GL11 
.11 
GetAttribLocation11 #
(11# $
Handle11$ *
,11* +

attribName11, 6
)116 7
;117 8
}22 
public44 

int44 
GetUniformLocation44 !
(44! "
string44" (

attribName44) 3
)443 4
{55 
return66 
GL66 
.66 
GetUniformLocation66 $
(66$ %
Handle66% +
,66+ ,

attribName66- 7
)667 8
;668 9
}77 
private99 
bool99 
disposedValue99 
=99  
false99! &
;99& '
	protected:: 
virtual:: 
void:: 
Dispose:: "
(::" #
bool::# '
	disposing::( 1
)::1 2
{;; 
if<< 

(<< 
!<< 
disposedValue<< 
)<< 
{== 	
GL>> 
.>> 
DeleteProgram>> 
(>> 
Handle>> #
)>># $
;>>$ %
disposedValue@@ 
=@@ 
true@@  
;@@  !
}AA 	
}BB 
publicDD 

voidDD 
GLFinishDD 
(DD 
)DD 
=>DD 
GLDD  
.DD  !
FinishDD! '
(DD' (
)DD( )
;DD) *
publicFF 

voidFF 
DisposeFF 
(FF 
)FF 
{GG 
DisposeHH 
(HH 
trueHH 
)HH 
;HH 
GCII 

.II
 
SuppressFinalizeII 
(II 
thisII  
)II  !
;II! "
}JJ 
}KK ˘
oC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLFallbackRenderStrategy.cs
	namespace 	
	ScottPlot
 
. 
OpenGL 
; 
public 
enum $
GLFallbackRenderStrategy $
{ 
Skip 
, 	
Software 
, 
} Ó
cC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\GLExtensions.cs
	namespace 	
	ScottPlot
 
; 
public 
static 
class 
GLExtensions  
{ 
public 

static 
OpenTK 
. 
Graphics !
.! "
Color4" (
	ToTkColor) 2
(2 3
this3 7
	ScottPlot8 A
.A B
ColorB G
colorH M
)M N
{ 
return 
new 
OpenTK 
. 
Graphics "
." #
Color4# )
() *
color* /
./ 0
Red0 3
,3 4
color5 :
.: ;
Green; @
,@ A
colorB G
.G H
BlueH L
,L M
colorN S
.S T
AlphaT Y
)Y Z
;Z [
} 
}		 –
mC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Controls\ScottPlot.OpenGL\AddPlottableExtensions.cs
	namespace 	
	ScottPlot
 
; 
public 
static 
class "
AddPlottableExtensions *
{		 
public 

static 

Plottables 
. 
	ScatterGL &
	ScatterGL' 0
(0 1
this1 5
PlottableAdder6 D
addE H
,H I
IPlotControlJ V
controlW ^
,^ _
double` f
[f g
]g h
xsi k
,k l
doublem s
[s t
]t u
ysv x
)x y
{ $
ScatterSourceDoubleArray  
source! '
=( )
new* -
(- .
xs. 0
,0 1
ys2 4
)4 5
;5 6
IScatterSource 
sourceWithCaching (
=) *
new+ .'
CacheScatterLimitsDecorator/ J
(J K
sourceK Q
)Q R
;R S

Plottables 
. 
	ScatterGL 
sp 
=  !
new" %
(% &
sourceWithCaching& 7
,7 8
control9 @
)@ A
;A B
Color 
	nextColor 
= 
add 
. 
GetNextColor *
(* +
)+ ,
;, -
sp 

.
 
	LineStyle 
. 
Color 
= 
	nextColor &
;& '
sp 

.
 
MarkerStyle 
. 
Fill 
. 
Color !
=" #
	nextColor$ -
;- .
add 
. 
	Plottable 
( 
sp 
) 
; 
return 
sp 
; 
} 
public 

static 

Plottables 
. 
ScatterGLCustom ,
ScatterGLCustom- <
(< =
this= A
PlottableAdderB P
addQ T
,T U
IPlotControlV b
controlc j
,j k
doublel r
[r s
]s t
xsu w
,w x
doubley 
[	 Ä
]
Ä Å
ys
Ç Ñ
)
Ñ Ö
{ $
ScatterSourceDoubleArray  
data! %
=& '
new( +
(+ ,
xs, .
,. /
ys0 2
)2 3
;3 4

Plottables 
. 
ScatterGLCustom "
sp# %
=& '
new( +
(+ ,
data, 0
,0 1
control2 9
)9 :
;: ;
Color   
	nextColor   
=   
add   
.   
GetNextColor   *
(  * +
)  + ,
;  , -
sp!! 

.!!
 
	LineStyle!! 
.!! 
Color!! 
=!! 
	nextColor!! &
;!!& '
sp"" 

.""
 
MarkerStyle"" 
."" 
Fill"" 
."" 
Color"" !
=""" #
	nextColor""$ -
;""- .
add## 
.## 
	Plottable## 
(## 
sp## 
)## 
;## 
return$$ 
sp$$ 
;$$ 
}%% 
}&& 