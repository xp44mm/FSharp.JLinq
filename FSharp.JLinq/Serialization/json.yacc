value : object
	  | array
	  | NULL
	  | FALSE
	  | TRUE
	  | STRING
	  | NUMBER
	  ;

object : "{" "}"
       | "{" fields "}"
       ;

array  : "[" "]"
       | "[" values "]"
       ;

fields : field
	   | fields "," field
	   ;

field : STRING ":" value
	  ;

values : value
	   | values "," value
	   ;


