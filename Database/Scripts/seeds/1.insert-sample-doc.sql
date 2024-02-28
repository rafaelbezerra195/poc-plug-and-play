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
    values (@request_type, @request_origin, 'purchaseOrder', 'Pedido de Compra', 0, 0, 1, GETDATE(), GETDATE());

    DECLARE @request_schema_id INT = 0;
    SELECT @request_schema_id = id from dbo.BMA_REQUEST_SCHEMA where Type = @request_type and Origin = @request_origin;

    -- INSERT TAB_SCHEMA
    INSERT INTO dbo.BMA_TAB_SCHEMA(RequestSchemaId, Name, Display, [Order], CreateDate, UpdateDate) 
    values (@request_schema_id, 'timeline', 'Timeline', 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'itens', "Itens", 2, GETDATE(), GETDATE()),
           (@request_schema_id, 'root', "Root", 0, GETDATE(), GETDATE());

    DECLARE @tab_schema_timeline_id INT = 0;
    SELECT @tab_schema_timeline_id = id from dbo.BMA_TAB_SCHEMA where Name = 'timeline'
    
    DECLARE @tab_schema_itens_id INT = 0;
    SELECT @tab_schema_itens_id = id from dbo.BMA_TAB_SCHEMA where Name = 'itens'

    DECLARE @tab_schema_root_id INT = 0;
    SELECT @tab_schema_root_id = id from dbo.BMA_TAB_SCHEMA where Name = 'root'

    -- INSERT FIELDS_SCHEMA
    INSERT INTO dbo.BMA_FIELD_SCHEMA(RequestSchemaId, Name, TabSchemaId, FieldSchemaTypesId, Required, Display, Visible, [Order], CreateDate, UpdateDate)
    values (@request_schema_id, 'type', @tab_schema_root_id,  @type_string, 1, "Type", 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'value', @tab_schema_root_id,  @type_currency, 1, "Value", 1, 2, GETDATE(), GETDATE()),
           (@request_schema_id, 'requester', @tab_schema_root_id,  @type_string, 1, "Requester", 1, 3, GETDATE(), GETDATE()),
           (@request_schema_id, 'status', @tab_schema_root_id,  @type_string, 1, "Status", 0, 4, GETDATE(), GETDATE()),
           (@request_schema_id, 'material', @tab_schema_itens_id,  @type_string, 0, "Material", 1, 4, GETDATE(), GETDATE()),
           (@request_schema_id, 'description', @tab_schema_timeline_id,  @type_string, 0, "Description", 1, 5, GETDATE(), GETDATE());

COMMIT TRANSACTION;