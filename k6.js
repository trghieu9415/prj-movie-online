import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
  scenarios: {
    booking_concurrency: {
      executor: 'constant-vus',
      vus: 3,
      duration: '1s',
          },
  },
};

const tokens = [
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE0MDkzZGZkLTlmZWQtNDgzOC05YjFjLWQwMjdmNGEwNDFiYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJWxakgSOG7r3UgQ8aw4budbmciLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyLm51bTBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJzZWN1cml0eV9zdGFtcCI6IkwyR0dMNE5ETk5ONEdCNzQ2WUkyTkZSREk3SEFTUlhNIiwidG9rZW5fdHlwZSI6ImFjY2VzcyIsImp0aSI6IjI0MGFkNGMzLWQxNjgtNDgwYi1iNTU0LTM4MGZlYjY0ODNiMSIsImV4cCI6MTc3NzE0MzAxNiwiaXNzIjoiTW92aWVPbmxpbmVBUEkiLCJhdWQiOiJNb3ZpZU9ubGluZUNsaWVudCJ9.d8TIER6EB765sUmqOC5EWO84RMlWo--bH2Q8fvsnu7A",
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjU2NDQzMjZiLTllNDMtNGIzYy1hMzFjLTBiYTY1ZDg1YTczYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiLEkOG6t25nIFF14bqjbmcgVGjDtG5nIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoidXNlci5udW0xQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkN1c3RvbWVyIiwic2VjdXJpdHlfc3RhbXAiOiJQNEg1WkZIT09GWTI3VkVES1RXVzZIUlJQQ1EzRlY2UiIsInRva2VuX3R5cGUiOiJhY2Nlc3MiLCJqdGkiOiI5MmY5ODA4OC00N2RhLTQwNjQtOTRhMy1lZmI4NmI3Yjc4MmYiLCJleHAiOjE3NzcxNDMwNTAsImlzcyI6Ik1vdmllT25saW5lQVBJIiwiYXVkIjoiTW92aWVPbmxpbmVDbGllbnQifQ.Gu9WGduaiZnUtJSxUzS-Tq5yhU7SQ5ADfN7gYb5LLjw",
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjA3ZDYwZDJmLTMyN2MtNGIzNy1iMDFjLTYzMDI3ZmZmOTgzMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiLEkOG6t25nIFTDrWNoIMOQ4bupYyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InVzZXIubnVtMkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDdXN0b21lciIsInNlY3VyaXR5X3N0YW1wIjoiSkNBVkQyR0dFMkxNQkRZSENVSlpaV0pTTjc0NDVWTUUiLCJ0b2tlbl90eXBlIjoiYWNjZXNzIiwianRpIjoiYzE0NmNmNGYtNDM4NC00OTlkLTk3NDktMTVkYTY1ZTcyM2RkIiwiZXhwIjoxNzc3MTQzMDYyLCJpc3MiOiJNb3ZpZU9ubGluZUFQSSIsImF1ZCI6Ik1vdmllT25saW5lQ2xpZW50In0.sr4MUCqziHZ2_c--nxQv0YLcJjKLN77wqrB8S4D3sRc"
  ];

export default function () {
  const currentToken = tokens[__VU - 1];
  const url = 'http://localhost:5202/api/user/order';

  const payload = JSON.stringify({
    showtimeId: "00bcc942-7551-4c5f-be12-30551d7f65bc",
    seatIds: [
      "18665840-cef3-42e7-b4ef-c6f4a74936dc"
    ]
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${currentToken}`,
    },
  };

  const res = http.post(url, payload, params);

  console.log(`============================================`);
  console.log(`[${new Date().toISOString()}] VU: ${__VU} - Iteration: ${__ITER + 1}`);
  console.log(`User Token: ${currentToken.substring(0, 10)}...`);
  console.log(`Status: ${res.status}`);

  try {
    const body = JSON.parse(res.body);
    console.log(`Response: \n${JSON.stringify(body, null, 2)}`);
  } catch (e) {
    console.log(`Response (Raw): ${res.body}`);
  }
  console.log(`============================================`);

  check(res, {
    'is status 200/201': (r) => r.status === 200 || r.status === 201,
    'is race condition (409)': (r) => r.status === 409,
  });
}
