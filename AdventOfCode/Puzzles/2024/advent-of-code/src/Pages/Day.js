import { useParams } from "react-router-dom";
import Solution from "../Solution";

const Home = () => {
	const dayNumber = useParams().day;

	return (
		<>
			<h1 className="text-center">
				<a
					className="small"
					href={`https://adventofcode.com/2024/day/${dayNumber}`}
					target="_blank"
					title={`AoC day ${dayNumber}`}
				>
					Day {dayNumber}{" "}
				</a>
			</h1>
			{/* <iframe
				src={`https://adventofcode.com/2024/day/${dayNumber}`}
				title={`Day ${dayNumber}`}
				style={{ width: "100%", height: "400px" }}
			/> */}
			<Solution day={dayNumber} />
		</>
	);
};

export default Home;
