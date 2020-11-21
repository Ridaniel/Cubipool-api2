

Feature: Registro
	Para poder usar el aplicativo
	como estudiante
	Quiero poder registrarme y loguearme con exito


Scenario: Registro realizado con exito
	Given mi usuario es U20161C809
	And contraseña es 12345678
	When ambos son correctos
	Then me debo poder registrar


Scenario: Login realizado con exito
	Given mi usuario  U20161C808
	And contraseña  12345678
	When ambos son correctos
	Then me sale un mensaje diciendo que se accedio correctamente

