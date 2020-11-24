

Feature: Buscar un cubiculo para reservar 
	Para ver los cubiculos disponibles
	como estudiante
	Quiero ver los cubiculos disponibles

Scenario:Busqueda correcta
Given una busqueda de un cubículo con 6 asientos
And la hora es de 8am a 11am
Then se presiona buscar 
When se muestran los cubiculos disponibles

Scenario:Busqueda vacia
Given una busqueda de un cubículo con 4 asientos
And la hora es de 8am a 11am
Then se presiona buscar 
When la lista de cubiculos esta vacia