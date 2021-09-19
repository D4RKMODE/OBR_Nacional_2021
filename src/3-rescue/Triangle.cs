char side_triangle = 'n';
int timeToFind = 0;

void Triangle () {

	bool TriRight () {
		if (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 90 && ultra(2) < 55) {
			side_triangle = 'R';
			return true;
		} else return false;
	}

	bool TriLeft () {
		if (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 90 && ultra(3) < 55) {
			side_triangle = 'L';
			return true;
		} else return false;
	}

	if (TriRight() || TriLeft()) {

		//verifies if is the triangle
			time.reset();
			float last_ultra = ultra(1);
			do {
				FollowerGyro();
				if (last_ultra - 2 > ultra(1)) return;
			} while (time.timer() < 150);
		//

		console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);

		//lifts the actuator and dispatches if it has a victim
			stop();
			actuator.Up();
			if (actuator.hasVictim() || actuator.hasKit()) {
				Dispatch();
			}
		//

		//position the robot in the side of the triangle
			GoToDistance(95);
			if (side_triangle == 'R') CentralizeGyro(-90);
			else CentralizeGyro(90);
			reverse(300, 750);
			actuator.Down();
		//

		//search for victims
			VictimInEnd:
			bool wall_ahead = (ultra(1) < 400);
			timeToFind = time.millis();
			while ((wall_ahead && !DetectWall()) || (!wall_ahead && isWhite(new byte[] {1,2,3,4}))) {
				FollowerGyro();
				Ultras(true, false, "triangle");
			}

			//wall or line
				actuator.Up();
				if (actuator.hasVictim()) {
					SearchTriangle(2, true);
					goto VictimInEnd;
				}
			//
		//

		Exit();

	}

}

