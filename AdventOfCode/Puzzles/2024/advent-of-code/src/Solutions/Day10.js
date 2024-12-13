import { DIRECTIONS, inputParser, UTILS } from "../solutionHelpers";

const Day10 = {
	part1: async (input) => {
		const map = inputParser.parseAsMultiDimentionArray(input, "", true, true);
		const trailHeads = [];
		for (let y = 0; y < map.length; y++) {
			for (let x = 0; x < map[y].length; x++) {
				if (map[y][x] === 0) {
					trailHeads.push({ x, y, score: 0 });
				}
			}
		}
		// find every path from a trailhead to a trailend where the value of the current position always increases by 1
		trailHeads.forEach((trailHead) => {
			const pathEnds = [];
			trailHead.score += Day10.findPaths(
				map,
				trailHead.x,
				trailHead.y,
				pathEnds
			);
		});
		return trailHeads.reduce((acc, trailHead) => trailHead.score + acc, 0);
	},
	part2: async (input) => {
		const map = inputParser.parseAsMultiDimentionArray(input, "", true, true);
		const trailHeads = [];
		for (let y = 0; y < map.length; y++) {
			for (let x = 0; x < map[y].length; x++) {
				if (map[y][x] === 0) {
					trailHeads.push({ x, y, score: 0 });
				}
			}
		}
		// find every path from a trailhead to a trailend where the value of the current position always increases by 1
		trailHeads.forEach((trailHead) => {
			trailHead.score += Day10.findPaths(map, trailHead.x, trailHead.y);
		});
		return trailHeads.reduce((acc, trailHead) => trailHead.score + acc, 0);
	},
	/**
	 *
	 * @param {number[][]} map
	 * @param {number} curX
	 * @param {number} curY
	 * @param {string[]?} pathEnds
	 * - if provided, will be filled with the coordinates of all the path ends found to only count unique endings
	 * @returns
	 */
	findPaths: (map, curX, curY, pathEnds = null) => {
		const curHeight = map[curY][curX];
		if (
			curHeight === 9 &&
			(!pathEnds || !pathEnds.includes(`${curX},${curY}`))
		) {
			pathEnds?.push(`${curX},${curY}`);
			return 1;
		}
		const nextHeight = curHeight + 1;
		const directions = [
			DIRECTIONS.up,
			DIRECTIONS.down,
			DIRECTIONS.left,
			DIRECTIONS.right,
		];
		let paths = 0;
		directions.forEach((dir) => {
			const nextPos = DIRECTIONS.getNextPos(dir, curX, curY);
			if (
				UTILS.checkIsInBounds(
					nextPos.x,
					nextPos.y,
					map.length,
					map[0].length
				) &&
				map[nextPos.y][nextPos.x] === nextHeight
			) {
				paths += Day10.findPaths(map, nextPos.x, nextPos.y, pathEnds);
			}
		});
		return paths;
	},
};

export default Day10;
