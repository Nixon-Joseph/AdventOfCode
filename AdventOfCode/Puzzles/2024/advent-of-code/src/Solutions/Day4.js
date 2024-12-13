import { DIRECTIONS, inputParser } from "../solutionHelpers";

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
		const lines = inputParser.parseAsLineArray(input.toUpperCase());
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

export default Day4;
