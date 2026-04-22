import { useRouteError, isRouteErrorResponse, Link } from 'react-router';
import { Button } from '@/components/ui/button';
import { AlertCircle, FileQuestion, ShieldAlert } from 'lucide-react';

export function ErrorBoundary() {
  const error = useRouteError();

  let errorMessage = 'An unexpected error occurred.';
  let status = 500;

  if (isRouteErrorResponse(error)) {
    status = error.status;
    errorMessage = error.data?.message || error.data || error.statusText;
  } else if (error instanceof Error) {
    errorMessage = error.message;
  }

  // Customize content based on status code
  let title = 'Server Error';
  let description = errorMessage;
  let Icon = AlertCircle;

  if (status === 404) {
    title = 'Page Not Found';
    description = "We couldn't find what you were looking for.";
    Icon = FileQuestion;
  } else if (status === 403) {
    title = 'Access Denied';
    description = "You don't have permission to view this resource.";
    Icon = ShieldAlert;
  } else if (status === 401) {
    title = 'Unauthorized';
    description = "Please log in to access this page.";
    Icon = ShieldAlert;
  }

  return (
    <div className="flex flex-col items-center justify-center py-20 text-center">
      <Icon className="h-24 w-24 text-muted-foreground mb-6" />
      <h1 className="text-4xl font-bold mb-2">{status} - {title}</h1>
      <p className="text-xl mb-8 text-muted-foreground">{description}</p>
      <Button asChild>
        <Link to="/">Return to Dashboard</Link>
      </Button>
    </div>
  );
}
