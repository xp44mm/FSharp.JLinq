value : object
	  | array
	  | NULL
	  | FALSE
	  | TRUE
	  | STRING
	  | NUMBER
	  ;

object : EMPTY_OBJECT
       | "(" fields ")"
       ;

fields : field
	   | fields "*" field
	   ;

field : KEY "!" value
	  ;

array : "(" ")"
      | "(" values ")"
      ;

values : value
	   | values "*" value
       ;