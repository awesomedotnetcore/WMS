SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE IT_LocalDetail;
TRUNCATE TABLE IT_LocalMaterial;
TRUNCATE TABLE IT_RecordBook;

TRUNCATE TABLE TD_Allocate;
TRUNCATE TABLE TD_AllocateDetail;
TRUNCATE TABLE TD_Bad;
TRUNCATE TABLE TD_BadDetail;
TRUNCATE TABLE TD_Check;
TRUNCATE TABLE TD_CheckArea;
TRUNCATE TABLE TD_CheckData;
TRUNCATE TABLE TD_CheckMaterial;
TRUNCATE TABLE TD_InStorDetail;
TRUNCATE TABLE TD_InStorage;
TRUNCATE TABLE TD_Move;
TRUNCATE TABLE TD_MoveDetail;
TRUNCATE TABLE TD_OutStorDetail;
TRUNCATE TABLE TD_OutStorage;
TRUNCATE TABLE TD_Receiving
TRUNCATE TABLE TD_RecDetail
TRUNCATE TABLE TD_Send
TRUNCATE TABLE TD_SendDetail

TRUNCATE TABLE PB_Address;
TRUNCATE TABLE PB_AreaMaterial;
TRUNCATE TABLE PB_BarCode;
TRUNCATE TABLE PB_BarCodeSerial;
TRUNCATE TABLE PB_Customer;
TRUNCATE TABLE PB_Equipment;
TRUNCATE TABLE PB_FeedPoint;
TRUNCATE TABLE PB_Laneway;
TRUNCATE TABLE PB_LocalTray;
TRUNCATE TABLE PB_Location;
TRUNCATE TABLE PB_Material;
TRUNCATE TABLE PB_MaterialPoint;
TRUNCATE TABLE PB_MaterialType;
TRUNCATE TABLE PB_Measure;
TRUNCATE TABLE PB_Rack;
TRUNCATE TABLE PB_StorArea;
TRUNCATE TABLE PB_Storage;
TRUNCATE TABLE PB_Supplier;
TRUNCATE TABLE PB_Tray;
TRUNCATE TABLE PB_TrayMaterial;
TRUNCATE TABLE PB_TrayType;
TRUNCATE TABLE PB_TrayZone;

TRUNCATE TABLE PD_Plan;


TRUNCATE TABLE Base_BuildTest;
TRUNCATE TABLE Base_DbLink;
TRUNCATE TABLE Base_Department;
TRUNCATE TABLE Base_Role;
TRUNCATE TABLE Base_RoleAction;
TRUNCATE TABLE Base_User;
TRUNCATE TABLE Base_UserLog;
TRUNCATE TABLE Base_UserRole;
TRUNCATE TABLE Base_UserStor;

SET FOREIGN_KEY_CHECKS = 1;


INSERT INTO `Base_User`(`Id`, `CreateTime`, `CreatorId`, `Deleted`, `UserName`, `Password`, `RealName`, `Sex`, `Birthday`, `DepartmentId`) VALUES ('Admin', NOW(), 'Admin', 0, 'Admin', 'e10adc3949ba59abbe56e057f20f883e', '超级管理员', 1, '2020-01-01', NULL);
INSERT INTO `Base_Department`(`Id`, `CreateTime`, `CreatorId`, `Deleted`, `Name`, `ParentId`) VALUES ('1', NOW(), 'Admin', 0, '集团总部', NULL);
INSERT INTO `Base_Role`(`Id`, `CreateTime`, `CreatorId`, `Deleted`, `RoleName`) VALUES ('1', NOW(), NULL, 0, '超级管理员');

INSERT INTO `Base_RoleAction`(`Id`, `CreateTime`, `CreatorId`, `Deleted`, `RoleId`, `ActionId`) 
SELECT CAST((@rownum :=@rownum+1) AS CHAR),NOW(),NULL,0,'1',Id FROM Base_Action t,(SELECT @rownum := 0) b ORDER BY t.Id;

INSERT INTO `Base_UserRole`(`Id`, `CreateTime`, `CreatorId`, `Deleted`, `UserId`, `RoleId`) VALUES ('1', NOW(), NULL, 0, 'Admin', '1');

INSERT INTO `PB_Storage`(`Id`, `Code`, `Name`, `Type`, `IsTray`, `IsZone`, `disable`, `IsDefault`, `Remarks`, `CreateTime`, `CreatorId`, `Deleted`) VALUES ('1', 'CK001', '默认仓库', 'Plane', 0, 0, 1, 0, NULL, NOW(), 'Admin', 0);

INSERT INTO `Base_UserStor`(`Id`, `UserId`, `StorId`, `IsDefault`, `CreateTime`, `CreatorId`, `Deleted`) VALUES ('1', '1', '1', 1, NOW(), 'Admin', 0);

INSERT INTO `PB_BarCodeSerial`(`Id`, `TypeId`, `ParaName`, `ParaValue`, `SerialNum`) VALUES ('1', '1265898274779303936', 'Serial', '', 1);