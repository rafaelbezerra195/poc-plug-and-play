USE PlugAndPlay

BEGIN TRANSACTION;

    INSERT INTO dbo.BMA_APPROVAL_RULES(name) 
    values ('sequencial'), 
        ('parallel');

    INSERT INTO dbo.BMA_FIELD_SCHEMA_TYPES(name) 
    values ('string'), 
        ('int'),
        ('currency'),
        ('file');

    DECLARE @type_string INT = 0;
    DECLARE @type_int INT = 0;
    DECLARE @type_currency INT = 0;
    DECLARE @type_file INT = 0;

    SELECT @type_string = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'string';
    SELECT @type_int = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'int';
    SELECT @type_currency = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'currency';
    SELECT @type_file = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'file';

    -- INSERT REQUEST SCHEMA
    DECLARE @request_type VARCHAR(2) = 'PC';
    DECLARE @request_origin VARCHAR(3) = 'SAP';

    INSERT INTO dbo.BMA_REQUEST_SCHEMA(Type, Origin, Name, Display, Active, AllowComments, ApprovalRuleId, CreateDate, UpdateDate) 
    values (@request_type, @request_origin, 'Pedido de Compra', false, false, 1, GETDATE(), GETDATE());

    DECLARE @request_schema_id INT = 0;
    SELECT @request_schema_id = id from dbo.BMA_REQUEST_SCHEMA where Type = @request_type and Origin = @request_origin;

    -- INSERT TAB_SCHEMA
    INSERT INTO dbo.BMA_TAB_SCHEMA(RequestSchemaId, Name, Display, Order, CreateDate, UpdateDate) 
    values (@request_schema_id, 'timeline', 'Timeline', true, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'itens', "Itens", true, 2, GETDATE(), GETDATE());

    DECLARE @tab_schema_timeline_id INT = 0;
    SELECT @tab_schema_timeline_id = id from dbo.BMA_TAB_SCHEMA where Name = 'timeline'
    
    DECLARE @tab_schema_itens_id INT = 0;
    SELECT @tab_schema_itens_id = id from dbo.BMA_TAB_SCHEMA where Name = 'itens'

    -- INSERT FIELDS_SCHEMA
    INSERT INTO dbo.BMA_FIELD_SCHEMA(RequestSchemaId, Name, TabSchemaId, FieldSchemaTypesId, Required, Display, Visible, Order, CreateDate, UpdateDate)
    values (@request_schema_id, 'type', null,  @type_string, true, "Type", true, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'value', null,  @type_currency, true, "Value", true, 2, GETDATE(), GETDATE()),
           (@request_schema_id, 'requester', null,  @type_string, true, "Requester", true, 3, GETDATE(), GETDATE()),
           (@request_schema_id, 'status', null,  @type_string, true, "Status", false, 4, GETDATE(), GETDATE()),
           (@request_schema_id, 'material', @tab_schema_itens_id,  @type_string, false, "Material", true, 4, GETDATE(), GETDATE()),
           (@request_schema_id, 'description', @tab_schema_timeline_id,  @type_string, false, "Description", true, 5, GETDATE(), GETDATE());

COMMIT TRANSACTION;