import { useEffect } from "react";
import { Outlet, Link, useLocation } from "react-router-dom";

const Layout = () => {
	const location = useLocation();
	useEffect(() => {}, [location]);
	const currentPath = location.pathname;

	return (
		<>
			<div className="d-flex flex-direction-column h-100">
				<nav
					style={{
						width: "150px",
						height: "100%",
						boxShadow: "4px 0px 10px 5px gray",
					}}
				>
					<ul className="nav flex-column nav-underline" style={{ gap: 0 }}>
						<li className="nav-item">
							<Link
								className={
									"nav-link px-2 py-1" + (currentPath === "/" ? " active" : "")
								}
								to="/"
							>
								Home
							</Link>
						</li>
						{Array.from(Array(25), (e, i) => {
							return (
								<li key={i} className="nav-item">
									<Link
										className={
											"nav-link px-2 py-1" +
											(currentPath === "/day/" + (i + 1) ? " active" : "")
										}
										to={{ pathname: "day/" + (i + 1).toString() }}
									>
										Day {i + 1}
									</Link>
								</li>
							);
						})}
					</ul>
				</nav>

				<div className="flex-direction-row p-4 w-100">
					<Outlet />
				</div>
			</div>
		</>
	);
};

export default Layout;
