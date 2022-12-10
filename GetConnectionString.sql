SELECT
    'data source=' + @@SERVERNAME +
    ';initial catalog=' + DB_NAME() +
    CASE type_desc
        WHEN 'WINDOWS_LOGIN' 
            THEN ';trusted_connection=true' + ';trust server certificate=true'
        ELSE
            ';user id=' + SUSER_NAME()
    END
FROM sys.server_principals
WHERE name = suser_name()