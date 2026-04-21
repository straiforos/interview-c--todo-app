import { useRouteError, isRouteErrorResponse, Link } from 'react-router';
import { Button } from '@/components/ui/button';

export function ErrorBoundary() {
  const error = useRouteError();

  let errorMessage = 'An unexpected error occurred.';
  let status = 500;

  if (isRouteErrorResponse(error)) {
    status = error.status;
    errorMessage = error.data?.message || error.statusText;
  } else if (error instanceof Error) {
    errorMessage = error.message;
  }

  return (
    <div className="flex flex-col items-center justify-center min-h-screen p-4 text-center">
      <h1 className="text-4xl font-bold mb-4">{status}</h1>
      <p className="text-xl mb-8 text-muted-foreground">{errorMessage}</p>
      <Button asChild>
        <Link to="/">Go Home</Link>
      </Button>
    </div>
  );
}
