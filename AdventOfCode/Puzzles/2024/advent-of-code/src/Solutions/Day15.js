import { DIRECTIONS, UTILS } from "../solutionHelpers";

const Day15 = {
	part1: async (input) => {
		const lines = input.split("\r\n");
		const instructions = [];
		const grid = [];
		const robot = { x: 0, y: 0 };
		let parsingGrid = true;
		lines.forEach((line, lineIndex) => {
			if (parsingGrid) {
				if (line === "") {
					parsingGrid = false;
				} else {
					grid.push(line.split(""));
					const robotIndex = line.indexOf("@");
					if (robotIndex !== -1) {
						robot.x = robotIndex;
						robot.y = lineIndex;
					}
				}
			} else {
				instructions.splice(
					instructions.length,
					0,
					...line.split("").filter((x) => x !== "")
				);
			}
		});

		await Day15.followInstructions(robot, instructions, grid);
		let gpsSum = 0;
		for (let y = 0; y < grid.length; y++) {
			for (let x = 0; x < grid[y].length; x++) {
				if (grid[y][x] === "O" || grid[y][x] === "O") {
					gpsSum += y * 100 + x;
				}
			}
		}
		console.log(grid.map((x) => x.join("")).join("\n"));
		return gpsSum;
	},
	tryMove: (x, y, direction, grid) => {
		const nextPos = DIRECTIONS.getNextPos(direction, x, y);
		const curPosOccupant = grid[y][x];
		if (
			UTILS.checkIsInBounds(nextPos.x, nextPos.y, grid[0].length, grid.length)
		) {
			if (grid[nextPos.y][nextPos.x] === "#") {
				return false;
			} else if (grid[nextPos.y][nextPos.x] === ".") {
				grid[nextPos.y][nextPos.x] = curPosOccupant;
				grid[y][x] = ".";
				return true;
			} else if (Day15.tryMove(nextPos.x, nextPos.y, direction, grid)) {
				grid[nextPos.y][nextPos.x] = curPosOccupant;
				grid[y][x] = ".";
				return true;
			}
		}
		return false;
	},
	tryMoveDoubled: (
		x,
		y,
		direction,
		grid,
		depth = 0,
		actionQueue = [],
		checkingSecondOfPair = false
	) => {
		const nextPos = DIRECTIONS.getNextPos(direction, x, y);
		const curPosOccupant = grid[y][x];
		let moveIsValid = false;
		if (
			UTILS.checkIsInBounds(nextPos.x, nextPos.y, grid[0].length, grid.length)
		) {
			if (grid[nextPos.y][nextPos.x] !== "#") {
				actionQueue.push({
					depth,
					action: () => {
						grid[nextPos.y][nextPos.x] = curPosOccupant;
						grid[y][x] = ".";
					},
				});
				if (
					!checkingSecondOfPair &&
					["[", "]"].includes(curPosOccupant) &&
					[DIRECTIONS.up, DIRECTIONS.down].includes(direction)
				) {
					if (curPosOccupant === "[") {
						moveIsValid =
							Day15.tryMoveDoubled(
								x + 1,
								y,
								direction,
								grid,
								depth + 1,
								actionQueue,
								true
							) &&
							Day15.tryMoveDoubled(
								x,
								y,
								direction,
								grid,
								depth + 1,
								actionQueue,
								true
							);
					} else if (curPosOccupant === "]") {
						moveIsValid =
							Day15.tryMoveDoubled(
								x - 1,
								y,
								direction,
								grid,
								depth + 1,
								actionQueue,
								true
							) &&
							Day15.tryMoveDoubled(
								x,
								y,
								direction,
								grid,
								depth + 1,
								actionQueue,
								true
							);
					}
				} else if (grid[nextPos.y][nextPos.x] === ".") {
					moveIsValid = true;
				} else if (
					Day15.tryMoveDoubled(
						nextPos.x,
						nextPos.y,
						direction,
						grid,
						depth + 1,
						actionQueue
					)
				) {
					moveIsValid = true;
				}
			}
			if (moveIsValid && depth === 0 && actionQueue.length > 0) {
				actionQueue.sort((a, b) => a.depth - b.depth);
				do {
					const { action } = actionQueue.pop();
					action();
				} while (actionQueue.length > 0);
			}
			return moveIsValid;
		}
	},
	followInstructions: async (
		robot,
		instructions,
		grid,
		doubleSized = false
	) => {
		for (let i = 0; i < instructions.length; i++) {
			let instruction = instructions[i];
			let direction;
			switch (instruction) {
				case "^":
					direction = DIRECTIONS.up;
					break;
				case "v":
					direction = DIRECTIONS.down;
					break;
				case "<":
					direction = DIRECTIONS.left;
					break;
				case ">":
					direction = DIRECTIONS.right;
					break;
				default:
					throw new Error("Invalid instruction");
			}
			if (doubleSized) {
				if (Day15.tryMoveDoubled(robot.x, robot.y, direction, grid)) {
					const nextPos = DIRECTIONS.getNextPos(direction, robot.x, robot.y);
					robot.x = nextPos.x;
					robot.y = nextPos.y;
				}
			} else if (Day15.tryMove(robot.x, robot.y, direction, grid)) {
				const nextPos = DIRECTIONS.getNextPos(direction, robot.x, robot.y);
				robot.x = nextPos.x;
				robot.y = nextPos.y;
			}
			console.log(
				`${grid
					.map((x) => x.join(""))
					.join("\n")}\n\nInstruction: ${instruction} - ${i + 1} of ${
					instructions.length
				}`
			);
		}
	},
	part2: async (input) => {
		const lines = input.split("\r\n");
		const instructions = [];
		const grid = [];
		const robot = { x: 0, y: 0 };
		let parsingGrid = true;
		lines.forEach((line, lineIndex) => {
			if (parsingGrid) {
				if (line === "") {
					parsingGrid = false;
				} else {
					const lineParts = line.split("");
					const newLine = [];
					for (let i = 0; i < lineParts.length; i++) {
						if (lineParts[i] === "@") {
							newLine.splice(newLine.length, 0, "@", ".");
						} else if (lineParts[i] === "O") {
							newLine.splice(newLine.length, 0, "[", "]");
						} else {
							newLine.splice(newLine.length, 0, lineParts[i], lineParts[i]);
						}
					}
					grid.push(newLine);
					const robotIndex = newLine.indexOf("@");
					if (robotIndex !== -1) {
						robot.x = robotIndex;
						robot.y = lineIndex;
					}
				}
			} else {
				instructions.splice(
					instructions.length,
					0,
					...line.split("").filter((x) => x !== "")
				);
			}
		});

		await Day15.followInstructions(robot, instructions, grid, true);
		let gpsSum = 0;
		for (let y = 0; y < grid.length; y++) {
			for (let x = 0; x < grid[y].length; x++) {
				if (grid[y][x] === "[") {
					gpsSum += y * 100 + x;
				}
			}
		}
		console.log(grid.map((x) => x.join("")).join("\n"));
		return gpsSum;
	},
};

export default Day15;
