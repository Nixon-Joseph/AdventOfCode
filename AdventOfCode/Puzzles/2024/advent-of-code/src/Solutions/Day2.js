import { inputParser } from "../solutionHelpers";

const Day2 = {
	setup: (input) => {
		return inputParser.parseAsMultiDimentionArray(input);
	},
	checkBaseLevelSafe: (levels) => {
		let safeReport = true;
		let direction = 0;
		for (let i = 1; i < levels.length; i++) {
			const diff = levels[i] - levels[i - 1];
			// first iteration, determine direction
			if (Math.abs(diff) > 3) {
				// if the diff is too big, unsafe.
				safeReport = false;
				break;
			} else if (diff === 0) {
				// if they match, unsafe.
				safeReport = false;
				break;
			} else if (diff > 0) {
				// trending up
				if (direction === -1) {
					// if trending up but previously was trending down, unsafe.
					safeReport = false;
					break;
				}
				direction = 1;
			} else if (diff < 0) {
				// trending down
				if (direction === 1) {
					// if trending down but previously was trending up, unsafe.
					safeReport = false;
					break;
				}
				direction = -1;
			}
		}
		return safeReport;
	},
	part1: async (input) => {
		const inputArrs = Day2.setup(input);
		let safeReports = 0;
		inputArrs.forEach((levels) => {
			if (Day2.checkBaseLevelSafe(levels)) {
				safeReports++;
			}
		});
		return safeReports;
	},
	part2: async (input) => {
		const inputArrs = Day2.setup(input);
		let safeReports = 0;
		inputArrs.forEach((levels) => {
			let safe = Day2.checkBaseLevelSafe(levels);
			let index = 0;
			while (!safe && index < levels.length) {
				const newLevels = [...levels];
				newLevels.splice(index, 1);
				safe = Day2.checkBaseLevelSafe(newLevels);
				index++;
			}
			if (safe) {
				safeReports++;
			}
		});
		return safeReports;
	},
};

export default Day2;
