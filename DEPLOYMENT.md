# Railway Deployment Checklist

## Pre-Deployment

- [ ] Railway project created and linked to GitHub repository
- [ ] PostgreSQL database provisioned on Railway
- [ ] Railway Storage Bucket provisioned
- [ ] Environment variables configured (see below)
- [ ] Database migrations applied
- [ ] Application builds successfully locally

## Environment Variables

Configure these in Railway Dashboard:

```
# Database
ConnectionStrings__DefaultConnection=<Railway PostgreSQL connection string>

# S3 Storage (Railway Storage)
S3Storage__ServiceUrl=<Railway Storage endpoint>
S3Storage__Region=us-east-1
S3Storage__AccessKey=<Railway Storage access key>
S3Storage__SecretKey=<Railway Storage secret key>
S3Storage__BucketName=<Your bucket name>

# Optional
ASPNETCORE_ENVIRONMENT=Production
```

## Deployment Steps

1. **Push code to GitHub**
   ```bash
   git add .
   git commit -m "Add S3 storage integration"
   git push origin main
   ```

2. **Railway will auto-deploy**
   - Monitor build logs in Railway dashboard
   - Check for any build errors

3. **Verify deployment**
   - Check health endpoint
   - Test S3 connectivity
   - Run a test CSV upload

## Post-Deployment Verification

- [ ] Application starts without errors
- [ ] Database connection successful
- [ ] S3 storage connection successful
- [ ] API endpoints responding
- [ ] CSV upload from S3 working

## Troubleshooting

If deployment fails:

1. Check Railway build logs
2. Verify all environment variables are set
3. Ensure S3 configuration is valid
4. Check database migration status
