import { inputParser } from "../solutionHelpers";

const Day14 = {
	parseRobot: (line) => {
		// p=9,5 v=-3,-3
		const match = /^p=([0-9]+),([0-9]+) v=(-?[0-9]+),(-?[0-9]+)/.exec(line);
		return {
			position: { x: parseInt(match[1]), y: parseInt(match[2]) },
			velocity: { x: parseInt(match[3]), y: parseInt(match[4]) },
		};
	},
	setup: (input, gridWidth, gridHeight) => {
		const lines = inputParser.parseAsLineArray(input);
		const robots = lines.map(Day14.parseRobot);
		const grid = Array.from(Array(gridHeight), () =>
			Array.from(Array(gridWidth), () => ".")
		);
		return { robots, grid };
	},
	getQuadrantProduct: (grid) => {
		const gridWidth = grid[0].length;
		const gridHeight = grid.length;
		const quadrantTotals = [0, 0, 0, 0];
		const quadBoundaryx = Math.floor(gridWidth / 2);
		const quadBoundaryy = Math.floor(gridHeight / 2);
		for (let y = 0; y < gridHeight; y++) {
			for (let x = 0; x < gridWidth; x++) {
				const value = grid[y][x];
				if (typeof value === "number") {
					if (x < quadBoundaryx && y < quadBoundaryy) {
						quadrantTotals[0] += value;
					} else if (x > quadBoundaryx && y < quadBoundaryy) {
						quadrantTotals[1] += value;
					} else if (x < quadBoundaryx && y > quadBoundaryy) {
						quadrantTotals[2] += value;
					} else if (x > quadBoundaryx && y > quadBoundaryy) {
						quadrantTotals[3] += value;
					}
				}
			}
		}
		return quadrantTotals.reduce((acc, val) => acc * val, 1);
	},
	part1: async (input) => {
		const gridWidth = 101,
			gridHeight = 103;
		const { robots, grid } = Day14.setup(input, gridWidth, gridHeight);
		const numSeconds = 100;
		robots.forEach((robot) => {
			robot.position.x += robot.velocity.x * numSeconds;
			robot.position.y += robot.velocity.y * numSeconds;
			// normalize position via teleports
			robot.position.x = robot.position.x % gridWidth;
			robot.position.y = robot.position.y % gridHeight;
			// adjust negative positions
			if (robot.position.x < 0) robot.position.x += gridWidth;
			if (robot.position.y < 0) robot.position.y += gridHeight;
			const gridValue = grid[robot.position.y][robot.position.x];
			grid[robot.position.y][robot.position.x] =
				typeof gridValue === "number" ? gridValue + 1 : 1;
		});
		console.log(grid.map((row) => row.join("")).join("\n"));
		const quadProduct = Day14.getQuadrantProduct(grid);
		return quadProduct;
	},
	part2: async (input) => {
		const gridWidth = 101,
			gridHeight = 103;
		const { robots } = Day14.setup(input, gridWidth, gridHeight);
		const numSeconds = 100_000;
		for (let i = 0; i < numSeconds; i++) {
			const gridCopy = Array.from(Array(gridHeight), () =>
				Array.from(Array(gridWidth), () => ".")
			);
			robots.forEach((robot) => {
				robot.position.x += robot.velocity.x;
				robot.position.y += robot.velocity.y;

				// normalize position via teleports
				if (robot.position.x >= gridWidth) robot.position.x -= gridWidth;
				if (robot.position.y >= gridHeight) robot.position.y -= gridHeight;
				if (robot.position.x < 0) robot.position.x += gridWidth;
				if (robot.position.y < 0) robot.position.y += gridHeight;
				gridCopy[robot.position.y][robot.position.x] = "X";
			});
			let foundConsecutive = false;
			let count = 0;
			const target = 10;
			for (let y = 0; y < gridHeight; y++) {
				for (let x = 0; x < gridWidth; x++) {
					if (gridCopy[y][x] === "X") {
						count++;
						if (count >= target) {
							foundConsecutive = true;
							break;
						}
					} else {
						count = 0;
					}
				}
				if (foundConsecutive) break;
			}
			if (foundConsecutive) {
				console.log(gridCopy.map((row) => row.join("")).join("\n"));
				return i + 1;
			}
		}
	},
};

export default Day14;
