SELECT setval(
    pg_get_serial_sequence('"Flows"', 'Id'),
    COALESCE(MAX("Id"), 1)
) 
FROM "Flows";

SELECT setval(
    pg_get_serial_sequence('"Realms"', 'Id'),
    COALESCE(MAX("Id"), 1)
) 
FROM "Realms";

SELECT setval(
    pg_get_serial_sequence('"FlowDetails"', 'Id'),
    COALESCE(MAX("Id"), 1)
)
FROM "FlowDetails";

SELECT setval(
    pg_get_serial_sequence('"Robots"', 'Id'),
    COALESCE(MAX("Id"), 1)
) 
FROM "Robots";

SELECT setval(
    pg_get_serial_sequence('"Waypoints"', 'Id'),
    COALESCE(MAX("Id"), 1)
) 
FROM "Waypoints";


SELECT setval(
    pg_get_serial_sequence('"RobotSystemInfos"', 'Id'),
    COALESCE(MAX("Id"), 1)
) 
FROM "RobotSystemInfos";

SELECT setval(
    pg_get_serial_sequence('"WaypointTraffics"', 'Id'),
    COALESCE(MAX("Id"), 1)
) 
FROM "WaypointTraffics";

