void DeadVictim (sbyte side_mod) {
	GoToDistance(95);
	CentralizeGyro(90 * side_mod);
	reverse(300, 750);
	moveZm(95);
	CentralizeGyro(-90 * side_mod);

	while (ultra(1) > 40) FollowerGyro();
	stop();
	open_actuator = false;
	actuator.Down();
	moveTime(200, 300);

	delay(500);
	moveTime(-300, 150);
	moveTime(300, 50);

	GoToDistance(95);
	CentralizeGyro(90 * side_mod);
	reverse(300, 1500);

	DeadVictimReserved = true;
	open_actuator = true;
}
