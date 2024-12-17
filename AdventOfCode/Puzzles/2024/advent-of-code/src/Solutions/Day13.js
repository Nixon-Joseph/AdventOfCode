import { inputParser } from "../solutionHelpers";

/**
 * Button A: X+94, Y+34
 * Button B: X+22, Y+67
 * Prize: X=8400, Y=5400
 *
 * multivariable algebra:
 *
 * XCALC: 8400 = 94a + 22b
 *
 * aFunc:
 * 8400 - 22b = 94a
 * (8400 - 22b) / 94 = a
 *
 * bFunc:
 * 8400 - 94a = 22b
 * (8400 - 94a) / 22 = b
 *
 *
 * YCALC: 5400 = 34a + 67b
 *
 * aFunc:
 * 5400 - 67b = 34a
 * (5400 - 67b) / 34 = a
 *
 * bFunc:
 * 5400 - 34a = 67b
 * (5400 - 34a) / 67 = b
 */

const Day13 = {
	parseButton: (btnText) => {
		const matches = /^Button (A|B): X\+([0-9]{1,}), Y\+([0-9]{1,})/.exec(
			btnText
		);
		const x = Number(matches[2]);
		const y = Number(matches[3]);
		return {
			x,
			y,
			cost: matches[1] === "A" ? 3 : 1,
			push: (pos) => {
				return {
					x: x + pos.x,
					y: y + pos.y,
				};
			},
			unpush: (pos) => {
				return {
					x: pos.x - x,
					y: pos.y - y,
				};
			},
		};
	},
	coordFunc: (target, a, b, x) => (target - b * x) / a,
	parsePrize: (prizeText) => {
		const matches = /^Prize: X=([0-9]{1,}), Y=([0-9]{1,})/.exec(prizeText);
		return {
			x: Number(matches[1]),
			y: Number(matches[2]),
		};
	},
	setup: (input) => {
		const lines = inputParser.parseAsLineArray(input);
		const machines = [];
		for (let i = 0; i < lines.length - 2; i += 3) {
			const buttonA = Day13.parseButton(lines[i]);
			const buttonB = Day13.parseButton(lines[i + 1]);
			const prize = Day13.parsePrize(lines[i + 2]);

			machines.push({
				buttonA,
				buttonB,
				prize,
			});
		}
		return machines;
	},
	testMachine: (prize, button1, button2) => {
		let aRes = 0;
		let bRes = 0;
		let lastDistance = Number.MAX_SAFE_INTEGER;
		let iteration = 0;
		let runCount = 0;
		// while the outputs are getting closer
		while (runCount < 2 || lastDistance > Math.abs(aRes - bRes)) {
			lastDistance =
				aRes === 0 && bRes === 0 ? lastDistance : Math.abs(aRes - bRes);
			aRes = Day13.coordFunc(prize.x, button1.x, button2.x, iteration);
			bRes = Day13.coordFunc(prize.y, button1.y, button2.y, iteration);
			if (aRes === bRes) {
				return button1.cost * aRes + button2.cost * iteration;
			}
			if (runCount > 2) {
				if (lastDistance > 10000000000) {
					iteration += 100000000;
				} else if (lastDistance > 1000000000) {
					iteration += 10000000;
				} else if (lastDistance > 100000000) {
					iteration += 1000000;
				} else if (lastDistance > 10000000) {
					iteration += 100000;
				} else if (lastDistance > 1000000) {
					iteration += 10000;
				} else if (lastDistance > 100000) {
					iteration += 1000;
				} else if (lastDistance > 10000) {
					iteration += 100;
				} else if (lastDistance > 1000) {
					iteration += 10;
				} else {
					iteration++;
				}
			} else {
				iteration++;
			}
			runCount++;
		}
		return 0;
	},
	processMachines: (machines) => {
		let totalCost = 0;
		machines.forEach((machine) => {
			const test = Day13.testMachine(
				machine.prize,
				machine.buttonA,
				machine.buttonB
			);
			if (test !== 0) {
				totalCost += test;
			}
		});
		return totalCost;
	},
	part1: async (input) => {
		const machines = Day13.setup(input);
		return Day13.processMachines(machines);
	},
	part2: async (input) => {
		const machines = Day13.setup(input);
		machines.forEach((machine) => {
			machine.prize.x += 10000000000000;
			machine.prize.y += 10000000000000;
		});
		return Day13.processMachines(machines);
	},
};

export default Day13;
