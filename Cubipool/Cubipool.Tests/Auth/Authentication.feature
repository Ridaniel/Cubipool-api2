Feature: Feature1
	Para poder usar el aplicativo
	como estudiante
	Quiero poder registrarme 

@Registro
Scenario: Registro realizado con exito
	Given mi usuario 
	And contraseña 
	When ambos son correctos
	Then me debo poder registrar

Scenario: No es un condigo de estudiante valido
	Given un codigo de estudiante
	When es invalido 
	Then me sale un mensaje diciendo que el codigo de estudiante es invalido

Scenario: Contraseña mas de 20 caracteres
	Given una contraseña 
	When tiene mas de 20 caracteres
	Then me sale un mensaje diciendo que la contraseña es muy larga

Scenario: Contraseña con el caracter de espacio
	Given una contraseña 
	When contiene el caracter de espacio
	Then me sale un mensaje diciendo que la contraseña no puede tener ese caracter

Scenario: Contraseña vacia
	Given una contraseña 
	When esta vacio
	Then me sale un mensaje diciendo que la contraseña no puede estar vacia

@Login

Scenario: Login realizado con existo
	Given mi usuario 
	And contraseña 
	When ambos son correctos
	Then me sale un mensaje diciendo que se accedio correctamente

Scenario: Login realizado con exito
	Given mi usuario 
	And contraseña 
	When ambos son correctos
	Then me sale un mensaje diciendo que se accedio correctamente