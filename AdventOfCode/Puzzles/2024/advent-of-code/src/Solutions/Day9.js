const Day9 = {
	generateChecksum: (diskRep) => {
		const _diskRep = diskRep.reduce(
			(acc, obj) =>
				acc.concat(
					Array.from(Array(obj.size), () =>
						obj.id === "FREESPACE" ? "." : obj.id
					)
				),
			[]
		);
		let checksum = 0;
		for (let i = 0; i < _diskRep.length; i++) {
			if (typeof _diskRep[i] === "string") {
				continue;
			}
			checksum += _diskRep[i] * i;
		}
		return checksum;
	},
	setup: (input) => {
		const diskBlocks = input.split("").map(Number);
		let diskRep = [];
		let blockId = 0;
		// build representation of disk data
		for (let i = 0; i < diskBlocks.length; i += 2) {
			const numBlocks = diskBlocks[i];
			const freeSpace = diskBlocks.length >= i ? diskBlocks[i + 1] : 0;
			diskRep.push({ id: blockId, size: numBlocks });
			if (freeSpace > 0) {
				diskRep.push({ id: "FREESPACE", size: freeSpace });
			}
			blockId++;
		}
		return diskRep;
	},
	part1: async (input) => {
		const diskRep = Day9.setup(input);
		// shift blocks to the left one block at a time
		for (let i = diskRep.length - 1; i >= 0; i--) {
			const block = diskRep[i];
			if (block.id === "FREESPACE") {
				diskRep.splice(i, 1);
				continue;
			}
			let firstFreeSpace = -1;
			do {
				firstFreeSpace = -1;
				for (let j = 0; j < diskRep.length; j++) {
					if (diskRep[j].id === "FREESPACE") {
						firstFreeSpace = j;
						break;
					}
				}
				if (firstFreeSpace >= 0) {
					let freeBlock = diskRep[firstFreeSpace];
					let newBlock = { ...block, size: 0 };
					while (block.size > 0 && freeBlock.size > 0) {
						newBlock.size++;
						block.size--;
						freeBlock.size--;
					}
					if (freeBlock.size === 0) {
						diskRep.splice(firstFreeSpace, 1);
						if (i > firstFreeSpace) {
							i--;
						}
					}
					if (newBlock.size > 0) {
						diskRep.splice(firstFreeSpace, 0, newBlock);
						if (i > firstFreeSpace) {
							i++;
						}
					}
					if (block.size === 0) {
						diskRep.splice(i, 1);
					}
				}
			} while (block.size > 0 && firstFreeSpace >= 0);
		}
		// generate checksum
		let checksum = Day9.generateChecksum(diskRep);
		return checksum;
	},
	part2: async (input) => {
		const diskRep = Day9.setup(input);
		// let printIndex = 0;
		// let printStr = diskRep
		// 	.map((x) => `${x.id === "FREESPACE" ? "." : x.id}:${x.size}`)
		// 	.join(",");
		const checkedIds = [];
		for (let i = diskRep.length - 1; i >= 0; i--) {
			const block = diskRep[i];
			if (block.id === "FREESPACE" || checkedIds.includes(block.id)) {
				continue;
			}
			checkedIds.push(block.id);
			let firstFreeSpace = -1;
			for (let j = 0; j < diskRep.length; j++) {
				if (diskRep[j].id === "FREESPACE" && diskRep[j].size >= block.size) {
					firstFreeSpace = j;
					break;
				}
			}
			if (firstFreeSpace >= 0 && firstFreeSpace < i) {
				const freeBlock = diskRep[firstFreeSpace];
				// if fully fills free space
				if (freeBlock.size === block.size) {
					// remove free space block
					diskRep.splice(firstFreeSpace, 1);
					// shift index if affected
					if (i > firstFreeSpace) {
						i--;
					}
				} else {
					// reduce free space block size
					freeBlock.size -= block.size;
				}
				// insert new block
				diskRep.splice(firstFreeSpace, 0, { ...block });
				// adjust index if affected
				if (i > firstFreeSpace) {
					i++;
				}
				// remove old block
				diskRep.splice(i, 1);
				// if the space before the block is a freespice, increase it's size
				if (i > 0 && diskRep[i - 1].id === "FREESPACE") {
					diskRep[i - 1].size += block.size;
					if (i < diskRep.length && diskRep[i].id === "FREESPACE") {
						// if new block at index is a free space, remove it because it's neighboring a free space
						diskRep[i - 1].size += diskRep.splice(i, 1)[0].size;
						i--;
					}
					// else if the space after the block is a freespice, increase it's size
				} else if (i < diskRep.length && diskRep[i].id === "FREESPACE") {
					diskRep[i].size += block.size;
					if (i < diskRep.length - 1 && diskRep[i + 1].id === "FREESPACE") {
						// if new block at index + 1 is a free space, remove it because it's neighboring a free space
						diskRep[i].size += diskRep.splice(i + 1, 1)[0].size;
					}
					// else insert a new free space block
				} else {
					diskRep.splice(i, 0, { id: "FREESPACE", size: block.size });
				}
				// printIndex = 0;
				// printStr +=
				// 	"\n" +
				// 	diskRep
				// 		.map((x) => `${x.id === "FREESPACE" ? "." : x.id}:${x.size}`)
				// 		.join(",");
			}
		}
		//6398065793754
		//6398065450842 // correct, from Isaac
		// console.log(printStr);
		const checksum = Day9.generateChecksum(diskRep);
		return checksum;
	},
};

export default Day9;
