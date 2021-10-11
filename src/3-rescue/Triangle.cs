char side_triangle = 'n';
int timeToFind = 0;

void Triangle () {

	bool TriRight () {
		if (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 97 && ultra(2) < 55) {
			side_triangle = 'R';
			return true;
		} else return false;
	}

	bool TriLeft () {
		if (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 97 && ultra(3) < 55) {
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

			console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);
			sbyte side_mod = (sbyte) (side_triangle == 'L' ? 1 : -1);
		//

		//lifts the actuator and dispatches if it has a victim
			stop();
			actuator.Up();
			if ((actuator.hasVictim() && actuator.isAlive()) || actuator.hasKit()) {
				bool wasKit = actuator.hasKit();
				Dispatch();
				if (!wasKit) AliveVictimsRescued++;
			} else if (actuator.hasVictim()) {
				DeadVictim(side_mod);
			}
		//

		//position the robot in the side of the triangle
			if (!DeadVictimReserved) {
				GoToDistance(95);
				CentralizeGyro(90 * side_mod);
				reverse(300, 750);
			}
			actuator.Down();
		//

		//search for victims
			byte timesSearched = 0;
			VictimInEnd:

			bool wall_ahead = (ultra(1) < 400);
			timeToFind = time.millis();
			while (((wall_ahead && !DetectWall()) || (!wall_ahead && isWhite(new byte[] {1,2,3,4}))) && time.millis() - timeToFind < 10000) {
				Ultras(true, false, "triangle");
			}

			int mid_arena = (int)((time.millis()-timeToFind)/2.5);
			if (time.millis() - timeToFind > 9950) {
				mid_arena = (time.millis()-timeToFind)/4;
			}

			//wall or line
				stop();
				actuator.Up();
				CentralizeGyro();
				if (actuator.hasVictim() && timesSearched < 3) {
					led(color["red"]);
					SearchTriangle(2, true);
					timesSearched++;
					goto VictimInEnd;
				} else {
					time.reset();
					while (time.timer() < 750) {
						Ultras(true, false, "triangle");
					}
				}
			//

		//

		//search for the exit
			console(2, $"{time.millis() - timeToFind} | {mid_arena}");

			if (DeadVictimReserved) { //rescue the remanescent dead victim
				DispatchDeadVictim(side_mod);
				moveTime(300, mid_arena);
			} else moveTime(-300, mid_arena);

			Exit(side_mod);
		//

	}

}

