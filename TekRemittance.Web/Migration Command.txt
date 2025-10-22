dotnet ef migrations add InitialCreate -p TekRemittance.Repository -s TekRemittance.Web

dotnet ef database update -p TekRemittance.Repository -s TekRemittance.Web

For update database scripts
dotnet ef migrations script 20251021092141_AcquisitionAgent 20251021094714_AcquisitionAgentUpdate --project TekRemittance.Repository --startup-project TekRemittance.Web -o migration.sql