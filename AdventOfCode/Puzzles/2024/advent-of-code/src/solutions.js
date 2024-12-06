const inputParser = {
	parseAsMultiDimentionArray: (
		input,
		splitChar = " ",
		removeEmpties = true,
		returnAsNumbers = false
	) => {
		return input.split("\r\n").map((line) =>
			line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => {
					const val = x.trim();
					return returnAsNumbers ? Number(val) : val;
				})
		);
	},
	parseAsLeftRightArrays: (
		input,
		splitChar = " ",
		removeEmpties = true,
		returnAsNumbers = false
	) => {
		const returnArrs = { left: [], right: [] };
		input.split("\r\n").forEach((line) => {
			const lineContents = line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => x.trim());
			if (lineContents.length === 2) {
				returnArrs.left.push(
					returnAsNumbers ? Number(lineContents[0]) : lineContents[0]
				);
				returnArrs.right.push(
					returnAsNumbers ? Number(lineContents[1]) : lineContents[1]
				);
			}
		});
		return returnArrs;
	},
	parseAsLineArray: (input, splitChar = "\r\n", removeEmpties = true) => {
		return input
			.split(splitChar)
			.filter((item) => (removeEmpties ? item !== "" : true))
			.map((x) => x.trim());
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
	getNextPos: (direction, x, y) => {
		switch (direction) {
			case DIRECTIONS.up:
				return { x, y: y - 1 };
			case DIRECTIONS.down:
				return { x, y: y + 1 };
			case DIRECTIONS.left:
				return { x: x - 1, y };
			case DIRECTIONS.right:
				return { x: x + 1, y };
			case DIRECTIONS.upLeft:
				return { x: x - 1, y: y - 1 };
			case DIRECTIONS.upRight:
				return { x: x + 1, y: y - 1 };
			case DIRECTIONS.downLeft:
				return { x: x - 1, y: y + 1 };
			case DIRECTIONS.downRight:
				return { x: x + 1, y: y + 1 };
			default:
				throw new Error("Invalid direction");
		}
	},
	getDirectionAfterTurn: (currentDirection, turn) => {
		switch (currentDirection) {
			case DIRECTIONS.up:
				return turn === "L" ? DIRECTIONS.left : DIRECTIONS.right;
			case DIRECTIONS.down:
				return turn === "L" ? DIRECTIONS.right : DIRECTIONS.left;
			case DIRECTIONS.left:
				return turn === "L" ? DIRECTIONS.down : DIRECTIONS.up;
			case DIRECTIONS.right:
				return turn === "L" ? DIRECTIONS.up : DIRECTIONS.down;
			default:
				throw new Error("Invalid direction");
		}
	},
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

const Day5 = {
	sortUpdateByRules: (currentUpdate, allRules) => {
		const sorted = [...currentUpdate].sort((a, b) => {
			const foundARules = allRules.filter(
				(x) => x.left === a && currentUpdate.includes(x.right)
			);
			const foundBRules = allRules.filter(
				(x) => x.left === b && currentUpdate.includes(x.right)
			);
			return foundARules.length > foundBRules.length ? -1 : 1;
		});
		return sorted;
	},
	getMiddleNumberOfCorrectlySortedUpdate: (currentUpdate, allRules) => {
		const sorted = Day5.sortUpdateByRules(currentUpdate, allRules);
		if (sorted.join("") === currentUpdate.join("")) {
			return currentUpdate[Math.floor(currentUpdate.length / 2)];
		}
		return 0;
	},
	setup: (input) => {
		const sections = input.split("\r\n\r\n");
		const rules = inputParser.parseAsLeftRightArrays(
			sections[0],
			"|",
			true,
			true
		);
		const ruleArr = rules.left.map((v, i) => ({
			left: v,
			right: rules.right[i],
		}));
		const updates = inputParser.parseAsMultiDimentionArray(
			sections[1],
			",",
			true,
			true
		);
		return { ruleArr, updates };
	},
	part1: async (input) => {
		const { ruleArr, updates } = Day5.setup(input);
		let middleSum = 0;
		updates.forEach((update) => {
			middleSum += Day5.getMiddleNumberOfCorrectlySortedUpdate(update, ruleArr);
		});
		return middleSum;
	},
	part2: async (input) => {
		const { ruleArr, updates } = Day5.setup(input);
		let middleSum = 0;
		updates.forEach((update) => {
			if (Day5.getMiddleNumberOfCorrectlySortedUpdate(update, ruleArr) === 0) {
				const sortedUpdate = Day5.sortUpdateByRules(update, ruleArr);
				middleSum += sortedUpdate[Math.floor(sortedUpdate.length / 2)];
			}
		});
		return middleSum;
	},
};

const Day6 = {
	patrolGuard: (grid, guardStartPosition, startDirection = DIRECTIONS.up) => {
		let direction = startDirection;
		let currentPosition = { ...guardStartPosition };
		const getPropToSet = (dir) => {
			switch (dir) {
				case DIRECTIONS.up:
					return "up";
				case DIRECTIONS.down:
					return "down";
				case DIRECTIONS.left:
					return "left";
				case DIRECTIONS.right:
					return "right";
			}
		};
		let propToSet = getPropToSet(direction);
		const checkIsInBounds = (x, y) => {
			return y >= 0 && x >= 0 && y < grid.length && x < grid[0].length;
		};
		do {
			if (grid[currentPosition.y][currentPosition.x][propToSet] === true) {
				return 1; // already been here in this direction, going to repeat
			}
			// mark as visited in this direction
			grid[currentPosition.y][currentPosition.x][propToSet] = true;
			const nextPos = DIRECTIONS.getNextPos(
				direction,
				currentPosition.x,
				currentPosition.y
			);
			if (!checkIsInBounds(nextPos.x, nextPos.y)) {
				break; // fell off map, finished.
			}
			// check if we hit an obstacle
			if (grid[nextPos.y][nextPos.x] === "#") {
				// if so change direction
				direction = DIRECTIONS.getDirectionAfterTurn(direction, "R");
				propToSet = getPropToSet(direction);
			} else {
				// otherwise move in the current direction
				currentPosition = nextPos;
			}
		} while (checkIsInBounds(currentPosition.x, currentPosition.y));
		return 0;
	},
	setup: (input) => {
		let grid = inputParser.parseAsMultiDimentionArray(input, "");
		let guardPosition = { x: 0, y: 0 };
		grid = grid.map((row, y) =>
			row.map((col, x) => {
				if (col === "^") {
					guardPosition.x = x;
					guardPosition.y = y;
				}
				return col === "#" ? col : { value: col };
			})
		);
		return { grid, guardPosition };
	},
	part1: async (input) => {
		let { grid, guardPosition } = Day6.setup(input);
		Day6.patrolGuard(grid, guardPosition);
		const totalVisited = grid.reduce((acc, row) => {
			return acc + row.filter((col) => Day6.cellIsVisited(col)).length;
		}, 0);
		return totalVisited;
	},
	cellIsVisited: (cell) => {
		return cell.up || cell.down || cell.left || cell.right;
	},
	part2: async (input) => {
		let { grid, guardPosition } = Day6.setup(input);
		const originalGrid = JSON.parse(JSON.stringify(grid));
		let foundLoopOptions = 0;
		Day6.patrolGuard(grid, guardPosition);
		// we know that the obstacle can only be placed on a cell that is visited to be of any effect.
		// limiting iterations to cells that have been visited.
		for (let y = 0; y < grid.length; y++) {
			for (let x = 0; x < grid[0].length; x++) {
				if (
					grid[y][x] === "#" ||
					!Day6.cellIsVisited(grid[y][x]) ||
					(x === guardPosition.x && y === guardPosition.y)
				) {
					continue;
				}
				let testObstructionGrid = JSON.parse(JSON.stringify(originalGrid));
				testObstructionGrid[y][x] = "#";
				foundLoopOptions += Day6.patrolGuard(testObstructionGrid, {
					...guardPosition,
				});
			}
		}
		return foundLoopOptions;
	},
};

export const Solutions = {
	Day1,
	Day2,
	Day3,
	Day4,
	Day5,
	Day6,
};
