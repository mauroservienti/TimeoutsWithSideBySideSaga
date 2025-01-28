# TimeoutsWithSideBySideSaga

The sample requires a PostgreSQL database. Run the following docker command to pull a PostgreSQL image and run it.

```bash
docker run -e POSTGRES_USER=db_user -e POSTGRES_PASSWORD=your_password -p 5432:5432 -d postgres
```

Start the `V7endpoint` first. It sends a message to start a saga, the saga requests a 2-minute timeout, and then it closes itself.
Start the `V8endpoint` and wait for the timeout to expire.
