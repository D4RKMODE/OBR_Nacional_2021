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
						while (ultra(whichSensor) > 360) left(1000*side_mod);
						while (ultra(whichSensor) < 360) right(1000*side_mod);
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
			while (time.timer() < timeDetecting/1.8) left(1000*side_mod);

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
			while (isWhite(new byte[] {2,3}) && time.timer() < 8000) FollowerGyro(direction());
			int timeToBack = time.timer();
			if (time.timer() > 7950) {
				timeToBack = 3500;
				rotate(500, 30);
				if (ultra(1) < 80) rotate(500, -60);
				while (isWhite(new byte[] {2,3})) FollowerGyro(direction());
			}
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

				//goes forward until be on the line
					//while (!isFullBlack(1) && !isFullBlack(2) && !isFullBlack(3) && !isFullBlack(4)) forward(300);
					while (isThatColor(1, "GREEN") || isThatColor(2, "GREEN") || isThatColor(3, "GREEN") || isThatColor(4, "GREEN") || isThatColor(1, "CYAN") || isThatColor(2, "CYAN") || isThatColor(3, "CYAN") || isThatColor(4, "CYAN")) forward(200);
					moveTime(300, 100);
				//

				//Centralize in the empty space
					stop();
					actuator.Up();
					while ((!isThatColor(2, "GREEN") || !isThatColor(2, "CYAN")) && (!isThatColor(3, "GREEN") || isThatColor(3, "CYAN"))) right(1000);
					CentralizeGyro();

					if (ultra(1) > 60) {
						rotate(500, -10);
						if (ultra(1) > 60) {
							rotate(500, 3);
						}
					}
					GoToDistance(23, true);
				//

				//returns to the line
					rotate(500, -45);
					while (!isWhite(4)) left(1000);
					rotate(500, -5);
					while (isWhite(new byte[] {2,3}) && !isOrtogonal()) left(1000);
					if (scaleAngle(direction()) > 25) {
						moveTime(300, 75);
						if (isWhite(new byte[] {1,2,3,4})) {
							rotate(500, -30);
						}
					}
					Centralize();
					stop();
					actuator.Down();
				//

				local = Local.exit;
			}
		//
	//

}
