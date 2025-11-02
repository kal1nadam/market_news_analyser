# 🧠 Market News Analyzer

A **Clean Architecture .NET Web API** project that automatically imports, analyzes, and broadcasts **stock market news**.  
It integrates **FinancialModelingPrep** as the news source, uses **OpenAI Responses API** for AI-driven sentiment and impact analysis, and delivers high-impact alerts to connected clients via **SignalR websockets** — all managed by **Hangfire recurring jobs** and **an in-memory event bus with outbox pattern**.

---

## 🚀 Features

### ⚙️ Architecture & Design
- **Clean Architecture + CQRS** design
- **.NET 8 Web API** using MediatR for commands/queries
- **Entity Framework Core (SQLite)** as ORM
- **In-memory event bus + Outbox pattern** for reliable message processing
- **Background jobs and hosted services** for async workflows

### 🧩 Components
- **SignalR** – Real-time websocket notifications at `/hubs/news`
- **Hangfire** – Dashboard + scheduled background jobs
- **FinancialModelingPrep (FMP)** – External stock news API
- **OpenAI API** – AI news analysis (impact evaluation)
- **Dockerized** – Ready for local or cloud deployment

---

## 🧱 Project Workflow

1. **News Import**
   - The `NewsImportJob` runs hourly (`0 * * * *`) via Hangfire
   - Fetches the latest stock news from **FinancialModelingPrep**
   - Saves them to the **SQLite** database

2. **Outbox Pattern**
   - Each imported news triggers a `NewsCreated` event
   - Event is saved in the `OutboxMessages` table

3. **Event Publishing**
   - The **OutboxPublisherService** scans for new events
   - Publishes them to an **in-memory event bus**

4. **AI Analysis & Notifications**
   - The **NewsAnalyzerHostedService** subscribes to `NewsCreated`
   - Calls the **NewsProcessor** which requests **OpenAI Response API**
   - AI evaluates impact (0–100%)
   - If **Impact ≥ 70%**, a websocket notification is broadcasted to `/hubs/news`

---

## 🧰 Technologies

| Category | Tools / Libraries |
|-----------|------------------|
| Framework | .NET 8 Web API |
| Architecture | Clean Architecture + CQRS |
| ORM | Entity Framework Core (SQLite) |
| Messaging | In-memory event bus + Outbox pattern |
| Background Jobs | Hangfire |
| Realtime | SignalR |
| External APIs | FinancialModelingPrep, OpenAI |
| Containerization | Docker / Docker Compose |

---

## 🧪 API Endpoints

Once the application is running, visit **`/swagger`** for interactive documentation.

| Endpoint | Method | Description | Success Criteria |
|-----------|---------|--------------|------------------|
| `/api/Health` | `GET` | Health check | Returns 200 → app is running |
| `/api/News` | `POST` | Manual trigger to import stock news | Returns 200 → FMP API key valid |
| `/api/News` | `GET` | Retrieve all stored news from database | Returns 200 → DB configured correctly |

---

## 🖥 Hangfire Dashboard

Accessible at: **[http://localhost:5080/hangfire](http://localhost:5080/hangfire)**  
> No authentication required in dev mode

**Recurring Jobs:**
- `NewsImportJob` – runs hourly (cron: `0 * * * *`)

---

## 🔑 Configuration

Before running, open `appsettings.json` and fill in the missing API keys:

```json
"NewsApi": {
  "ApiKey": "financial_modeling_prep_api_key"
},
"OpenAiApi": {
  "ApiKey": "openai_api_key"
}
```

**Get your API keys:**
- FinancialModelingPrep → [https://site.financialmodelingprep.com/developer/docs/dashboard](https://site.financialmodelingprep.com/developer/docs/dashboard)
- OpenAI → [https://platform.openai.com/settings/organization/api-keys](https://platform.openai.com/settings/organization/api-keys)

---

## 🐳 Running the Project

### 1️⃣ Clone the repository
```bash
git clone https://github.com/kal1nadam/market_news_analyser.git
cd market_news_analyser
```

### 2️⃣ Build and run via Docker Compose
```bash
docker compose -f docker-compose.dev.yml up --build
```

### 3️⃣ Open in browser
- Swagger UI → `http://localhost:5080/swagger`
- Hangfire Dashboard → `http://localhost:5080/hangfire`
- SignalR Hub → `/hubs/news`

---

## 🧠 Example Flow

```
[Hangfire Job] -> imports news from FMP
   ↓
[Database] -> stores news + NewsCreated event in OutboxMessages
   ↓
[OutboxPublisherService] -> publishes event to in-memory bus
   ↓
[NewsAnalyzerHostedService] -> processes new news
   ↓
[OpenAI API] -> analyzes sentiment & impact
   ↓
[SignalR Hub] -> sends notification if Impact ≥ 70%
```

---

## 🧾 Example Notification (Websocket Message)

```json
{
  "newsId": "c9b9f3a7-4b5a-46a4-9327-7817c2f8b3b5",
  "tickerSymbol": "AAPL",
  "headline": "Apple Reports Record Q4 Earnings Exceeding Expectations",
  "createdAt": "2025-11-02T13:45:00Z",
  "impactPercentage": 82.5,
  "marketTrend": "Bullish",
  "reasonForMarketTrend": "Strong iPhone sales and positive forward guidance boosted investor sentiment."
}
```

---

## 🧩 Example Technologies Stack Diagram

```
[FMP] → [NewsImportJob → Outbox → Publisher → Analyzer → OpenAI]
                                                    ↓
                                             [SignalR WebSocket]
                                                    ↓
                                               [Frontend/UI]
```

---

## 🧑‍💻 Developer Notes

- Outbox processing ensures **eventual consistency** between DB and message bus.
- SignalR Hub is unauthenticated for simplicity — add **JWT** or **API key auth** in production.
- Hangfire Dashboard is publicly accessible in dev — protect with credentials in production.

---

## 🪄 Future Improvements

- Add user subscriptions for personalized stock alerts
- Add persistent message queue (e.g., RabbitMQ / Kafka)
- Store AI analysis history
- Add sentiment trend analysis
- Add unit tests

---

## 📜 License

MIT License © 2025 — Adam Kalina
