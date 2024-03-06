USE PlugAndPlay

BEGIN TRANSACTION;

    DECLARE @type_string INT = 0;
    DECLARE @type_group INT = 0;
    DECLARE @type_currency INT = 0;

    SELECT @type_string = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'string';
    SELECT @type_group = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'group';
    SELECT @type_currency = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'currency';

    -- INSERT REQUEST SCHEMA
    DECLARE @request_type VARCHAR(2) = 'PC';
    DECLARE @request_origin VARCHAR(3) = 'SAP';

    DECLARE @request_schema_id INT = 0;
    SELECT @request_schema_id = id from dbo.BMA_REQUEST_SCHEMA where Type = @request_type and Origin = @request_origin;
    
    DECLARE @tab_schema_itens_id INT = 0;
    SELECT @tab_schema_itens_id = id from dbo.BMA_TAB_SCHEMA where Name = 'itens'

    -- INSERT FIELDS_SCHEMA
    INSERT INTO dbo.BMA_FIELD_SCHEMA(RequestSchemaId, Name, TabSchemaId, FieldSchemaTypesId, Required, Display, Visible, [Order], CreateDate, UpdateDate)
    VALUES (@request_schema_id, 'itens', @tab_schema_itens_id,  @type_group, 0, 'Itens', 1, 1, GETDATE(), GETDATE());

    DECLARE @field_schema_group_itens INT = 0;
    SELECT @field_schema_group_itens = id from dbo.BMA_FIELD_SCHEMA where name = 'itens';

    INSERT INTO dbo.BMA_FIELD_SCHEMA(RequestSchemaId, Name, TabSchemaId, FieldSchemaTypesId, Required, Display, Visible, [Order], CreateDate, UpdateDate, Parent)
    VALUES (@request_schema_id, 'description', @tab_schema_itens_id,  @type_string, 0, 'Description', 1, 1, GETDATE(), GETDATE(), @field_schema_group_itens),
           (@request_schema_id, 'value', @tab_schema_itens_id,  @type_currency, 0, 'Value', 1, 2, GETDATE(), GETDATE(), @field_schema_group_itens);
           

COMMIT TRANSACTION;