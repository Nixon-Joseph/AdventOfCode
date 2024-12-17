import { DIRECTIONS, inputParser } from "../solutionHelpers";
import * as PF from "pathfinding";

const Day16 = {
	part1: async (input) => {
		const maze = inputParser.parseAsMultiDimentionArray(input, "", true);
		const grid = new PF.Grid(maze[0].length, maze.length);
		let start;
		let end;
		for (let y = 0; y < maze.length; y++) {
			for (let x = 0; x < maze[y].length; x++) {
				const curVal = maze[y][x];
				if (curVal === "S") {
					start = { x, y };
				} else if (curVal === "E") {
					end = { x, y };
				}
				grid.setWalkableAt(x, y, curVal === "#" ? false : true);
			}
		}
		let finder = new PF.AStarFinder({
			allowDiaognal: false,
			avoidStaircase: true,
			turnPenalty: 100000,
		});
		let path = finder.findPath(start.x, start.y, end.x, end.y, grid);
		console.log(path);
		let smoothenedPath = PF.Util.expandPath(PF.Util.smoothenPath(grid, path));
		console.log(smoothenedPath);
		var points = 0;
		let lastDir = DIRECTIONS.right;
		let lastNode = path[0];
		path.slice(1).forEach((node) => {
			let dir = DIRECTIONS.determineDirection(
				{ x: lastNode[0], y: lastNode[1] },
				{ x: node[0], y: node[1] }
			);
			if (dir !== lastDir) {
				points += 1000;
			}
			lastNode = node;
			lastDir = dir;
			points += 1;
		});
		return points;
	},
};

export default Day16;
