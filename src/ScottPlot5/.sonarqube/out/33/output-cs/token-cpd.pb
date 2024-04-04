Ü
]C:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.WinForms\Program.cs
	namespace 	
Sandbox
 
. 
WinForms 
; 
static 
class 
Program 
{ 
[ 
	STAThread 
] 
static		 

void		 
Main		 
(		 
)		 
{

 $
ApplicationConfiguration  
.  !

Initialize! +
(+ ,
), -
;- .
Application 
. 
Run 
( 
new 
Form1 !
(! "
)" #
)# $
;$ %
} 
} î
[C:\Users\there\source\ScottPlot\src\ScottPlot5\ScottPlot5 Sandbox\Sandbox.WinForms\Form1.cs
	namespace 	
Sandbox
 
. 
WinForms 
; 
public 
partial 
class 
Form1 
: 
Form !
{ 
public 

Form1 
( 
) 
{ 
InitializeComponent		 
(		 
)		 
;		 

formsPlot1 
. 
Plot 
. 
Add 
. 
Signal "
(" #
Generate# +
.+ ,
Sin, /
(/ 0
)0 1
)1 2
;2 3

formsPlot1 
. 
Plot 
. 
Add 
. 
Signal "
(" #
Generate# +
.+ ,
Cos, /
(/ 0
)0 1
)1 2
;2 3
} 
} 