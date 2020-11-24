

Feature: Reservar 
	Para poder usar el aplicativo
	como estudiante
	Quiero poder reservar un cubiculo


Scenario: Reserva realizado con exito
	Given mi usuarioId es 2 
	And tiempo de inicio es en una hora
	And tiempo de fin es en 2 horas
	Then se genera una reserva

Scenario: Usuario sin reserva
	Given mi usuarioId es 2 
	And tiempo de inicio es a las 5 am
	And tiempo de fin es 2 horas despues
	Then no se permite la reserva