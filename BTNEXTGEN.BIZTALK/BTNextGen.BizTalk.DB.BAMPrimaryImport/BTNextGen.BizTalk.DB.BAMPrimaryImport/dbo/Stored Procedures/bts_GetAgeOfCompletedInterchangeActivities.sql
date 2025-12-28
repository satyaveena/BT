create procedure [dbo].[bts_GetAgeOfCompletedInterchangeActivities]
AS
BEGIN
	declare @ageOfIsa int, @ageOfIaa int, @ageOfFgi int, @ageOfFaa int
	select @ageOfIsa = datediff(mi, min(isa.LastModified), getutcdate()) from bam_InterchangeStatusActivity_Completed isa
	select @ageOfIaa = datediff(mi, min(iaa.LastModified), getutcdate()) from bam_InterchangeAckActivity_Completed iaa
	select @ageOfFgi = datediff(mi, min(fgi.LastModified), getutcdate()) from bam_FunctionalGroupInfo_Completed fgi 
	select @ageOfFaa = datediff(mi, min(faa.LastModified), getutcdate()) from bam_FunctionalAckActivity_Completed faa
	select @ageOfIsa AgeOfISA, @ageOfIaa AgeOfIAA, @ageOfFgi AgeOfFGI, @ageOfFaa AgeOfFAA
END