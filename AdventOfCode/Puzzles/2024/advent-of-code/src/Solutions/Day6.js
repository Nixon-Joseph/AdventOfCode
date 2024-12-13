import { DIRECTIONS, inputParser } from "../solutionHelpers";

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
				default:
					throw new Error("Invalid direction");
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

export default Day6;
