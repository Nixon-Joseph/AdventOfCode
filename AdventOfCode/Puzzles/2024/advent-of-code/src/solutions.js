const inputParser = {
	parseAsMultiDimentionArray: (
		input,
		splitChar = " ",
		removeEmpties = true
	) => {
		return input.split("\r\n").map((line) =>
			line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => x.trim())
		);
	},
	parseAsLeftRightArrays: (input, splitChar = " ", removeEmpties = true) => {
		const returnArrs = { left: [], right: [] };
		input.split("\r\n").forEach((line) => {
			const lineContents = line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => x.trim());
			if (lineContents.length === 2) {
				returnArrs.left.push(lineContents[0]);
				returnArrs.right.push(lineContents[1]);
			}
		});
		return returnArrs;
	},
};

const Day1 = {
	setup: (input) => {
		const { left, right } = inputParser.parseAsLeftRightArrays(input);
		left.sort();
		right.sort();
		return { left, right };
	},
	part1: async (input) => {
		const { left, right } = Day1.setup(input);
		let total = 0;
		for (let i = 0; i < left.length; i++) {
			const diff = Math.abs(left[i] - right[i]);
			total += diff;
		}
		return total;
	},
	part2: async (input) => {
		const { left, right } = Day1.setup(input);
		let simScore = 0;
		left.forEach((item) => {
			const foundInRight = right.filter((x) => x === item);
			simScore += item * foundInRight.length;
		});
		return simScore;
	},
};

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

const Day3 = {
	part1: async (input) => {
		const foundMatches = input.match(/mul\([0-9]{1,3},\s?[0-9]{1,3}\)/g);
		let sum = 0;
		foundMatches.forEach((mul) => {
			const [a, b] = mul.match(/[0-9]{1,3}/g);
			sum += a * b;
		});
		return sum;
	},
	part2: async (input) => {
		let modifiedInput = input;
		const dontRegex = /don't\(\)/;
		while (dontRegex.test(modifiedInput)) {
			const nextDont = modifiedInput.match(dontRegex);
			const nextDo = modifiedInput.indexOf("do()", nextDont.index + 6);
			modifiedInput =
				modifiedInput.substring(0, nextDont.index) +
				modifiedInput.substring(nextDo + 4);
		}
		return Day3.part1(modifiedInput);
	},
};

const DIRECTIONS = {
	up: 0,
	down: 1,
	left: 2,
	right: 3,
	upLeft: 4,
	upRight: 5,
	downLeft: 6,
	downRight: 7,
};

const Day4 = {
	checkForTarget: (inputLines, target, direction, x, y) => {
		let nextElFunc = null;
		switch (direction) {
			case DIRECTIONS.right:
				nextElFunc = (i) => inputLines[y][x + i];
				break;
			case DIRECTIONS.left:
				nextElFunc = (i) => inputLines[y][x - i];
				break;
			case DIRECTIONS.down:
				nextElFunc = (i) => inputLines[y + i][x];
				break;
			case DIRECTIONS.up:
				nextElFunc = (i) => inputLines[y - i][x];
				break;
			case DIRECTIONS.downRight:
				nextElFunc = (i) => inputLines[y + i][x + i];
				break;
			case DIRECTIONS.downLeft:
				nextElFunc = (i) => inputLines[y + i][x - i];
				break;
			case DIRECTIONS.upRight:
				nextElFunc = (i) => inputLines[y - i][x + i];
				break;
			case DIRECTIONS.upLeft:
				nextElFunc = (i) => inputLines[y - i][x - i];
				break;
			default:
				throw new Error("Invalid direction");
		}
		for (let i = 0; i < target.length; i++) {
			if (target[i] !== nextElFunc(i)) {
				break;
			}
			if (i === target.length - 1) {
				return true;
			}
		}
		return false;
	},
	setup: (input) => {
		const lines = input.toUpperCase().split("\r\n");
		const lineLength = lines[0].length;
		const numLines = lines.length;
		return [lines, lineLength, numLines];
	},
	part1: async (input) => {
		const searchTarget = ["X", "M", "A", "S"];
		let foundTotal = 0;
		const [lines, lineLength, numLines] = Day4.setup(input);
		const targetLengthCheck = searchTarget.length - 1;
		// nested loop to check each character in each line
		// search in each direction to find the whole search target
		// any matches - increment foundTotal
		for (let y = 0; y < numLines; y++) {
			for (let x = 0; x < lineLength; x++) {
				// Found X, check all 8 directions
				if ("X" === lines[y][x]) {
					// check which directions are possible based on position
					// check right
					if (x + targetLengthCheck < lineLength) {
						Day4.checkForTarget(lines, searchTarget, DIRECTIONS.right, x, y) &&
							foundTotal++;
					}
					// check left
					if (x - targetLengthCheck >= 0) {
						Day4.checkForTarget(lines, searchTarget, DIRECTIONS.left, x, y) &&
							foundTotal++;
					}
					// check down
					if (y + targetLengthCheck < numLines) {
						Day4.checkForTarget(lines, searchTarget, DIRECTIONS.down, x, y) &&
							foundTotal++;
					}
					// check up
					if (y - targetLengthCheck >= 0) {
						Day4.checkForTarget(lines, searchTarget, DIRECTIONS.up, x, y) &&
							foundTotal++;
					}
					// check down-right
					if (
						x + targetLengthCheck < lineLength &&
						y + targetLengthCheck < numLines
					) {
						Day4.checkForTarget(
							lines,
							searchTarget,
							DIRECTIONS.downRight,
							x,
							y
						) && foundTotal++;
					}
					// check down-left
					if (x - targetLengthCheck >= 0 && y + targetLengthCheck < numLines) {
						Day4.checkForTarget(
							lines,
							searchTarget,
							DIRECTIONS.downLeft,
							x,
							y
						) && foundTotal++;
					}
					// check up-right
					if (
						x + targetLengthCheck < lineLength &&
						y - targetLengthCheck >= 0
					) {
						Day4.checkForTarget(
							lines,
							searchTarget,
							DIRECTIONS.upRight,
							x,
							y
						) && foundTotal++;
					}
					// check up-left
					if (x - targetLengthCheck >= 0 && y - targetLengthCheck >= 0) {
						Day4.checkForTarget(lines, searchTarget, DIRECTIONS.upLeft, x, y) &&
							foundTotal++;
					}
				}
			}
		}
		return foundTotal;
	},
	part2: async (input) => {
		const searchTarget = ["M", "A", "S"];
		const [lines, lineLength, numLines] = Day4.setup(input);
		let foundTotal = 0;
		for (let y = 1; y < numLines - 1; y++) {
			for (let x = 1; x < lineLength - 1; x++) {
				// Found A, check both cross directions
				if ("A" === lines[y][x]) {
					if (
						(Day4.checkForTarget(
							lines,
							searchTarget,
							DIRECTIONS.downRight,
							x - 1,
							y - 1
						) ||
							Day4.checkForTarget(
								lines,
								searchTarget,
								DIRECTIONS.upLeft,
								x + 1,
								y + 1
							)) &&
						(Day4.checkForTarget(
							lines,
							searchTarget,
							DIRECTIONS.downLeft,
							x + 1,
							y - 1
						) ||
							Day4.checkForTarget(
								lines,
								searchTarget,
								DIRECTIONS.upRight,
								x - 1,
								y + 1
							))
					) {
						foundTotal++;
					}
				}
			}
		}
		return foundTotal;
	},
};

export const Solutions = {
	Day1,
	Day2,
	Day3,
	Day4,
};
