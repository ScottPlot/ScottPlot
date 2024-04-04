Þ
XC:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Eto\Program.cs
	namespace 	
Sandbox
 
. 
Eto 
{ 
static 

class 
Program 
{ 
[ 	
	STAThread	 
] 
static		 
void		 
Main		 
(		 
string		 
[		  
]		  !
args		" &
)		& '
{

 	
new 
Application 
( 
global "
::" $
Eto$ '
.' (
Platform( 0
.0 1
Detect1 7
)7 8
.8 9
Run9 <
(< =
new= @

MainWindowA K
(K L
)L M
)M N
;N O
} 	
} 
} ¾
_C:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Eto\MainWindow.eto.cs
	namespace 	
Sandbox
 
. 
Eto 
; 
public 
partial 
class 

MainWindow 
:  !
Form" &
{ 
private 
readonly 
EtoPlot 
EtoPlot1 %
=& '
new( +
(+ ,
), -
;- .
private

 
void

 
InitializeComponent

 $
(

$ %
)

% &
{ 
Content 
= 
EtoPlot1 
; 
Width 
= 
$num 
; 
Height 
= 
$num 
; 
} 
} Â
[C:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.Eto\MainWindow.cs
	namespace 	
Sandbox
 
. 
Eto 
; 
partial 
class 

MainWindow 
: 
Form 
{ 
public 


MainWindow 
( 
) 
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
 
EtoPlot1 
. 
Plot 
. 
Add 
. 
Signal  
(  !
Generate! )
.) *
Sin* -
(- .
). /
)/ 0
;0 1
EtoPlot1 
. 
Plot 
. 
Add 
. 
Signal  
(  !
Generate! )
.) *
Cos* -
(- .
). /
)/ 0
;0 1
EtoPlot1 
. 
Refresh 
( 
) 
; 
} 
} 