void Exit (sbyte side_mod) {

	//verifies if already is in front of a empty space
		for (byte i = 0; i < 4; i++) {
			if (ultra(i) > 360) {
				while (ultra(i) > 400) left(1000 * side_mod);
			}
		}
	//

	FindTheExitAgain:
	//goes to the exit
		led(color["white"]);

		//identify which sensor reads more than the limit
			byte whichSensor = 0;
			while (whichSensor == 0) {
				right(1000 * side_mod);
				for (byte i = 0; i < 4; i++) {
					if (ultra(i) > 400) {
						whichSensor = i;
						break;
					}
				}
			}
			console(2, $"{whichSensor}");
		//

		//centralizes with the empty space
			AlreadyFindedTheExit:

			time.reset();
			while (ultra(whichSensor) > 360) right(1000*side_mod);
			int timeDetecting = time.timer();

			time.reset();
			while (time.timer() < timeDetecting/1.75) left(1000*side_mod);

			if (whichSensor == 2) rotate(500, 90);
			else if (whichSensor == 3) rotate(500, -90);

			if (ultra(1) < 400) {
				goto FindTheExitAgain;
			}

			stop();
			open_actuator = false;
			actuator.Down();
		//

		//goes to the empty space
			time.reset();
			while (isWhite(new byte[] {2,3})) FollowerGyro(direction());
			int timeToBack = time.timer();
			moveTime(300, 50);
		//

		//verifies if that is the entrance or the exit
		if (anySensorColor("blue") && !isThatColor(1, "GREEN") && !isThatColor(2, "GREEN") && !isThatColor(3, "GREEN") && !isThatColor(4, "GREEN") && !isThatColor(2, "CYAN") && !isThatColor(3, "CYAN")) {
			led(color["orange"]);
			moveTime(-300, timeToBack);
			if (whichSensor == 2) rotate(500, -90*side_mod);
			else if (whichSensor == 3) rotate(500, 90*side_mod);
			while (ultra(whichSensor) > 360) {
				right(1000*side_mod);
				for (byte i = 0; i < 4; i++) {
					if (ultra(i) > 400 && i != whichSensor) {
						whichSensor = i;
						goto AlreadyFindedTheExit;
					}
				}
			}
			right(1000*side_mod);
			delay(350);
			goto FindTheExitAgain;
		} else {
			led(color["green_dark"]);
			while (!isFullBlack(1) && !isFullBlack(2) && !isFullBlack(3) && !isFullBlack(4)) forward(300);
			while (isThatColor(1, "GREEN") || isThatColor(2, "GREEN") || isThatColor(3, "GREEN") || isThatColor(4, "GREEN")) forward(200);
			moveTime(300, 75);
			if (!anySensorLine()) Centralize();
			else {
				moveTime(300, 150);
				if (scaleAngle(direction()) > 30) {
					actuator.Up();
					while (isWhite(new byte[] {2,3})) right(1000);
					if (isThatColor(1, "GREEN") || isThatColor(4, "GREEN")) {
						rotate(500, -45);
						moveTime(-300, 100);
						while (isWhite(new byte[] {2,3})) left(1000);
						Centralize();
					}
					stop();
					actuator.Down();
				}
			}
			local = Local.exit;
		}
	//

}
