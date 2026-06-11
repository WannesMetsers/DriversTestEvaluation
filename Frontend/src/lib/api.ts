import type {
 DrivingEvent,
 DrivingEventResponseDto,
 Results,
 UpdateDto,
} from "@/types";

const apiUrl =import.meta.env.VITE_API_URL;
class ApiClient {

  private getSessionId(): string | null {
    return localStorage.getItem("session_id")

  }
  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const headers: HeadersInit = {
      "Content-Type": "application/json",
      ...options.headers,
    };

    const fullUrl = `${apiUrl}${endpoint}`;
    console.log(`[API] ▶️ ${options.method || 'GET'} ${fullUrl}`);
    
    if (options.body) {
      console.log(`[API] 📦 Payload:`, JSON.parse(options.body as string));
    }

    const response = await fetch(fullUrl, {
      ...options,
      headers,
    });

    console.log(`[API] ⬅️ Status: ${response.status} ${response.statusText}`);

    // Handle 204 No Content
    if (response.status === 204) {
      console.log(`[API] ✅ No Content (204)`);
      return {} as T;
    }

    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: "Er is een fout opgetreden" }));
      console.error(`[API] ❌ Error:`, error);
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
  }

    // Handle empty response body (some endpoints return 200 with no content)
if (response.status === 204) {
  return undefined as T;
}

const contentType = response.headers.get("content-type") || "";

if (contentType.includes("application/json")) {
  return await response.json();
}

return await response.text() as unknown as T;
  }
  // ============================================================
  // Get Update Driving Events
  // Get /api/session/{id}/getUpdate
  // ============================================================

  async GetUpdate(id: string) : Promise<UpdateDto>{
    const update : UpdateDto = await this.request(`/session/${id}/getUpdate`);
    console.log("result in api =: ", update);
    return update;
  }

  // ============================================================
  // Get TestResult
  // Get /api/session/{id}/Results
  // ============================================================

  async GetTestResult(id: string) : Promise<Results>{
    return await this.request(`/session/${id}/Results`);
  }

  // ============================================================
  // Start Test
  // Post /api/session/start
  // ============================================================

async StartTest(): Promise<string> {
  const result = await this.request<string>(
    "/session/start",
    { method: "POST" }
  );
  console.log("result in api =: ", result);

  return result;
}

  // ============================================================
  // Stop Test
  // Post /api/session/Stop
  // ============================================================

  async StopTest(id: string) {
   await this.request(`/session/${id}/stop`, { method: "POST" });
  }

}

export const api = new ApiClient();
