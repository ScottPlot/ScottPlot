ñH
eC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.ConsoleFramework\Program.cs
	namespace 	
Sandbox
 
. 
ConsoleFramework "
{ 
public 

static 
class 
Program 
{ 
public 
static 
void 
Main 
(  
)  !
{ 	 
Test_DoubleToGeneric		  
(		  !
)		! "
;		" # 
Test_GenericToDouble

  
(

  !
)

! "
;

" #
} 	
public 
static 
void  
Test_DoubleToGeneric /
(/ 0
)0 1
{ 	
	ScottPlot 
. 
NumericConversion '
.' (
DoubleToGeneric( 7
(7 8
$num8 >
,> ?
out@ C
doubleD J
vDoubleK R
)R S
;S T
Console 
. 
	WriteLine 
( 
vDouble %
)% &
;& '
	ScottPlot 
. 
NumericConversion '
.' (
DoubleToGeneric( 7
(7 8
$num8 >
,> ?
out@ C
floatD I
vSingleJ Q
)Q R
;R S
Console 
. 
	WriteLine 
( 
vSingle %
)% &
;& '
	ScottPlot 
. 
NumericConversion '
.' (
DoubleToGeneric( 7
(7 8
$num8 >
,> ?
out@ C
intD G
vInt32H N
)N O
;O P
Console 
. 
	WriteLine 
( 
vInt32 $
)$ %
;% &
	ScottPlot 
. 
NumericConversion '
.' (
DoubleToGeneric( 7
(7 8
$num8 >
,> ?
out@ C
uintD H
vUint32I P
)P Q
;Q R
Console 
. 
	WriteLine 
( 
vUint32 %
)% &
;& '
	ScottPlot 
. 
NumericConversion '
.' (
DoubleToGeneric( 7
(7 8
$num8 >
,> ?
out@ C
longD H
vInt64I O
)O P
;P Q
Console 
. 
	WriteLine 
( 
vInt64 $
)$ %
;% &
	ScottPlot 
. 
NumericConversion '
.' (
DoubleToGeneric( 7
(7 8
$num8 >
,> ?
out@ C
ulongD I
vUint64J Q
)Q R
;R S
Console 
. 
	WriteLine 
( 
vUint64 %
)% &
;& '
	ScottPlot!! 
.!! 
NumericConversion!! '
.!!' (
DoubleToGeneric!!( 7
(!!7 8
$num!!8 >
,!!> ?
out!!@ C
short!!D I
vInt16!!J P
)!!P Q
;!!Q R
Console"" 
."" 
	WriteLine"" 
("" 
vInt16"" $
)""$ %
;""% &
	ScottPlot$$ 
.$$ 
NumericConversion$$ '
.$$' (
DoubleToGeneric$$( 7
($$7 8
$num$$8 >
,$$> ?
out$$@ C
ushort$$D J
vUint16$$K R
)$$R S
;$$S T
Console%% 
.%% 
	WriteLine%% 
(%% 
vUint16%% %
)%%% &
;%%& '
	ScottPlot'' 
.'' 
NumericConversion'' '
.''' (
DoubleToGeneric''( 7
(''7 8
$num''8 >
,''> ?
out''@ C
decimal''D K
vDecimal''L T
)''T U
;''U V
Console(( 
.(( 
	WriteLine(( 
((( 
vDecimal(( &
)((& '
;((' (
	ScottPlot** 
.** 
NumericConversion** '
.**' (
DoubleToGeneric**( 7
(**7 8
$num**8 >
,**> ?
out**@ C
byte**D H
vByte**I N
)**N O
;**O P
Console++ 
.++ 
	WriteLine++ 
(++ 
vByte++ #
)++# $
;++$ %
	ScottPlot-- 
.-- 
NumericConversion-- '
.--' (
DoubleToGeneric--( 7
(--7 8
$num--8 =
,--= >
out--? B
DateTime--C K
	vDateTime--L U
)--U V
;--V W
Console.. 
... 
	WriteLine.. 
(.. 
	vDateTime.. '
)..' (
;..( )
}// 	
public11 
static11 
void11  
Test_GenericToDouble11 /
(11/ 0
)110 1
{22 	
double33 
vDouble33 
=33 
$num33 #
;33# $
Console44 
.44 
	WriteLine44 
(44 
	ScottPlot44 '
.44' (
NumericConversion44( 9
.449 :
GenericToDouble44: I
(44I J
ref44J M
vDouble44N U
)44U V
)44V W
;44W X
float66 
vSingle66 
=66 
$num66 #
;66# $
Console77 
.77 
	WriteLine77 
(77 
	ScottPlot77 '
.77' (
NumericConversion77( 9
.779 :
GenericToDouble77: I
(77I J
ref77J M
vSingle77N U
)77U V
)77V W
;77W X
int99 
vInt3299 
=99 
$num99 
;99 
Console:: 
.:: 
	WriteLine:: 
(:: 
	ScottPlot:: '
.::' (
NumericConversion::( 9
.::9 :
GenericToDouble::: I
(::I J
ref::J M
vInt32::N T
)::T U
)::U V
;::V W
uint<< 
vUint32<< 
=<< 
$num<<  
;<<  !
Console== 
.== 
	WriteLine== 
(== 
	ScottPlot== '
.==' (
NumericConversion==( 9
.==9 :
GenericToDouble==: I
(==I J
ref==J M
vUint32==N U
)==U V
)==V W
;==W X
long?? 
vInt64?? 
=?? 
$num?? 
;??  
Console@@ 
.@@ 
	WriteLine@@ 
(@@ 
	ScottPlot@@ '
.@@' (
NumericConversion@@( 9
.@@9 :
GenericToDouble@@: I
(@@I J
ref@@J M
vInt64@@N T
)@@T U
)@@U V
;@@V W
ulongBB 
vUint64BB 
=BB 
$numBB !
;BB! "
ConsoleCC 
.CC 
	WriteLineCC 
(CC 
	ScottPlotCC '
.CC' (
NumericConversionCC( 9
.CC9 :
GenericToDoubleCC: I
(CCI J
refCCJ M
vUint64CCN U
)CCU V
)CCV W
;CCW X
shortEE 
vInt16EE 
=EE 
$numEE  
;EE  !
ConsoleFF 
.FF 
	WriteLineFF 
(FF 
	ScottPlotFF '
.FF' (
NumericConversionFF( 9
.FF9 :
GenericToDoubleFF: I
(FFI J
refFFJ M
vInt16FFN T
)FFT U
)FFU V
;FFV W
ushortHH 
vUint16HH 
=HH 
$numHH "
;HH" #
ConsoleII 
.II 
	WriteLineII 
(II 
	ScottPlotII '
.II' (
NumericConversionII( 9
.II9 :
GenericToDoubleII: I
(III J
refIIJ M
vUint16IIN U
)IIU V
)IIV W
;IIW X
decimalKK 
vDecimalKK 
=KK 
newKK "
decimalKK# *
(KK* +
$numKK+ 1
)KK1 2
;KK2 3
ConsoleLL 
.LL 
	WriteLineLL 
(LL 
	ScottPlotLL '
.LL' (
NumericConversionLL( 9
.LL9 :
GenericToDoubleLL: I
(LLI J
refLLJ M
vDecimalLLN V
)LLV W
)LLW X
;LLX Y
byteNN 
vByteNN 
=NN 
$numNN 
;NN 
ConsoleOO 
.OO 
	WriteLineOO 
(OO 
	ScottPlotOO '
.OO' (
NumericConversionOO( 9
.OO9 :
GenericToDoubleOO: I
(OOI J
refOOJ M
vByteOON S
)OOS T
)OOT U
;OOU V
DateTimeQQ 
	vDateTimeQQ 
=QQ  
newQQ! $
DateTimeQQ% -
(QQ- .
$numQQ. 2
,QQ2 3
$numQQ4 5
,QQ5 6
$numQQ7 8
)QQ8 9
;QQ9 :
ConsoleRR 
.RR 
	WriteLineRR 
(RR 
	ScottPlotRR '
.RR' (
NumericConversionRR( 9
.RR9 :
GenericToDoubleRR: I
(RRI J
refRRJ M
	vDateTimeRRN W
)RRW X
)RRX Y
;RRY Z
}SS 	
}TT 
}UU 