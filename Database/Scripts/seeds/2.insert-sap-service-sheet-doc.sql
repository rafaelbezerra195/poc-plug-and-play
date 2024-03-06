USE PlugAndPlay

BEGIN TRANSACTION;

    DECLARE @type_string INT = 0;
    DECLARE @type_int INT = 0;
    DECLARE @type_currency INT = 0;
    DECLARE @type_file INT = 0;

    SELECT @type_string = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'string';
    SELECT @type_int = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'int';
    SELECT @type_currency = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'currency';
    SELECT @type_file = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'file';

    -- INSERT REQUEST SCHEMA
    DECLARE @request_type VARCHAR(2) = 'FS';
    DECLARE @request_origin VARCHAR(3) = 'SAP';

    INSERT INTO dbo.BMA_REQUEST_SCHEMA(Type, Origin, Name, Display, Active, AllowComments, ApprovalRuleId, CreateDate, UpdateDate) 
    values (@request_type, @request_origin, 'serviceSheet', 'Folha de Serviço', 0, 0, 1, GETDATE(), GETDATE());

    DECLARE @request_schema_id INT = 0;
    SELECT @request_schema_id = id from dbo.BMA_REQUEST_SCHEMA where Type = @request_type and Origin = @request_origin;

    -- INSERT TAB_SCHEMA
    INSERT INTO dbo.BMA_TAB_SCHEMA(RequestSchemaId, Name, Display, [Order], CreateDate, UpdateDate) 
    values (@request_schema_id, 'root', 'Root', 0, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline', 'Timeline', 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'attachments', 'Attachments', 2, GETDATE(), GETDATE());

    DECLARE @tab_schema_timeline_id INT = 0;
    SELECT @tab_schema_timeline_id = id from dbo.BMA_TAB_SCHEMA where Name = 'root'
    
    DECLARE @tab_schema_itens_id INT = 0;
    SELECT @tab_schema_itens_id = id from dbo.BMA_TAB_SCHEMA where Name = 'timeline'

    DECLARE @tab_schema_root_id INT = 0;
    SELECT @tab_schema_root_id = id from dbo.BMA_TAB_SCHEMA where Name = 'attachments'

    -- INSERT FIELDS_SCHEMA
    INSERT INTO dbo.BMA_FIELD_SCHEMA(RequestSchemaId, Name, TabSchemaId, FieldSchemaTypesId, Required, Display, Visible, [Order], CreateDate, UpdateDate)
    values (@request_schema_id, 'requestKey', @tab_schema_root_id,  @type_string, 1, 'Document Number', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'value', @tab_schema_root_id,  @type_currency, 1, 'Value', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'order', @tab_schema_root_id,  @type_currency, 0, '{ "pt": "Pedido", "en": "Order", "es": "Pedido"}', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.description', @tab_schema_timeline_id,  @type_string, 0, 'Description', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.value', @tab_schema_timeline_id,  @type_currency, 0, 'Description', 1, 1, GETDATE(), GETDATE());

COMMIT TRANSACTION;